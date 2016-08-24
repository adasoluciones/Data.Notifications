using Ada.Framework.Core;
using Ada.Framework.Data.Notifications.Business;
using Ada.Framework.Data.Notifications.Exceptions;
using Ada.Framework.Data.Notifications.TO;
using Ada.Framework.Extensions.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ada.Framework.Data.Notifications
{
    /// <summary>
    /// Representa la respuesta de un método, encapsulando la respuesta original y los mensajes que se desea desplegar a capa vista.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    /// <typeparam name="T">Tipo del objeto a envolver.</typeparam>
    public class Notificacion<T> : Notificacion
    {
        /// <summary>
        /// Permite obtener o establecer el objeto de respuesta que contiene la instancia actual de Notificacion.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public T Respuesta { get; set; }
    }

    /// <summary>
    /// Representa una notificación con una colección de mensajes para mostrar al usuario.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public class Notificacion
    {
        /// <summary>
        /// Tabla a la por defecto para buscar los mensajes comunes de la aplicación.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static readonly string TABLA_POR_DEFECTO_MENSAJES = "MSJE";

        /// <summary>
        /// Permite obtener o establecer el idioma actual del hilo.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static string IdiomaActual
        {
            get
            {
                string idiomaActual = Thread.CurrentThread.Obtener("IdiomaActual") as string;
                if(string.IsNullOrEmpty(idiomaActual))
                {
                    idiomaActual = "ES";
                    IdiomaActual = idiomaActual;
                }
                return idiomaActual;
            }

            set
            {
                Thread.CurrentThread.Guardar("IdiomaActual", value);
            }
        }

        /// <summary>
        /// Permite obtener o establecer el pais actual (para el idioma) del hilo.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static string PaisActual
        {
            get
            {
                string paisActual = Thread.CurrentThread.Obtener("PaisActual") as string;
                if (string.IsNullOrEmpty(paisActual))
                {
                    paisActual = "*";
                    PaisActual = paisActual;
                }
                return paisActual;
            }

            set
            {
                Thread.CurrentThread.Guardar("PaisActual", value);
            }
        }

        /// <summary>
        /// Lista de mensajes cargada desde base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        internal static IList<MensajeTO> mensajes;

        /// <summary>
        /// Obtiene un valor booleano que indica si existe un mensaje de error.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public bool HayError { get { return Hay(Severidad.Error); } }

        /// <summary>
        /// Obtiene un valor booleano que indica si existe un mensaje originado por una excepción.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public bool HayExcepcion { get { return mensajesInstancia.Count(c => c.Excepcion != null) > 0; } }

        /// <summary>
        /// Mensajes que posee la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        protected IList<MensajeTO> mensajesInstancia;

        /// <summary>
        /// Permite obtener la lista de mensajes que contiene la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IList<MensajeTO> Mensajes { get { return mensajesInstancia; } }

        /// <summary>
        /// Constructor de la clase que inicializa las propiedades de la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public Notificacion()
        {
            if (mensajes == null) mensajes = new List<MensajeTO>();
            mensajesInstancia = new List<MensajeTO>();
        }

        /// <summary>
        /// Constructor de la clase que inicializa las propiedades de la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <typeparam name="T">Tipo del código de la enumeración.</typeparam>
        /// <param name="tabla">Tabla de mensajes.</param>
        /// <param name="codigo">Código del mensaje.</param>
        /// <param name="campo">Campo asociado al error.</param>
        /// <param name="puntero">MethodGUID de Log4Me.</param>
        /// <param name="idioma">Idioma del mensaje.</param>
        /// <param name="pais">Pais específico para el idioma del mensaje.</param>
        /// <param name="excepcion">Excepción lanzada.</param>
        /// <param name="razonTecnica">Glosa técnica o explicación de la excepción.</param>
        public virtual MensajeTO AgregarMensaje<T>(string tabla, Enumeracion<T> codigo, string campo = null, string puntero = null, string idioma = null, string pais = null, Exception excepcion = null, string razonTecnica = null)
        {
            string codigoMensaje = codigo != null ? codigo.Codigo.ToString() : null;
            return AgregarMensaje(tabla, codigoMensaje, campo, puntero, idioma, pais, excepcion, razonTecnica);
        }

        /// <summary>
        /// Agregar un mensaje estático a la notificación.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="glosa">Glosa del mensaje.</param>
        /// <param name="tipo">Nivel de severidad del mensaje.</param>
        /// <param name="campo">Campo asociado al error.</param>
        /// <param name="puntero">MethodGUID de Log4Me.</param>
        /// <param name="excepcion">Excepción lanzada.</param>
        /// <param name="razonTecnica">Glosa técnica o explicación de la excepción.</param>
        public virtual MensajeTO AgregarMensaje(string glosa, Severidad tipo, string campo = null, string puntero = null, Exception excepcion = null, string razonTecnica = null)
        {
            if (glosa != null) glosa = glosa.Trim();
            if (campo != null) campo = campo.Trim();
            if (razonTecnica != null) razonTecnica = razonTecnica.Trim(); else if (excepcion != null) razonTecnica = excepcion.Message;

            MensajeTO retorno = new MensajeTO();
            retorno.Puntero = puntero;
            retorno.Glosa = glosa;
            retorno.Tipo = tipo;
            retorno.Campo = campo;
            retorno.Excepcion = excepcion;
            retorno.RazonTecnica = razonTecnica;
            
            mensajesInstancia.Add(retorno);
            return retorno;
        }

        /// <summary>
        /// Constructor de la clase que inicializa las propiedades de la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="tabla">Tabla de mensajes.</param>
        /// <param name="codigo">Código del mensaje.</param>
        /// <param name="campo">Campo asociado al error.</param>
        /// <param name="puntero">MethodGUID de Log4Me.</param>
        /// <param name="idioma">Idioma del mensaje.</param>
        /// <param name="pais">País específico para el idioma del mensaje.</param>
        /// <param name="excepcion">Excepción lanzada.</param>
        /// <param name="razonTecnica">Glosa técnica o explicación de la excepción.</param>
        public virtual MensajeTO AgregarMensaje(string tabla, string codigo, string campo = null, string puntero = null, string idioma = null, string pais = null, Exception excepcion = null, string razonTecnica = null)
        {
            if (tabla != null) tabla = tabla.Trim();
            if (codigo != null) codigo = codigo.Trim();
            if (campo != null) campo = campo.Trim();
            if (idioma != null) idioma = idioma.Trim();
            if (pais != null) pais = pais.Trim();

            if (razonTecnica != null) razonTecnica = razonTecnica.Trim(); else if (excepcion != null) razonTecnica = excepcion.Message;
            
            if (string.IsNullOrEmpty(tabla)) tabla = TABLA_POR_DEFECTO_MENSAJES;
            if (string.IsNullOrEmpty(pais)) pais = PaisActual;

            if (string.IsNullOrEmpty(idioma))
            {
                idioma = IdiomaActual;
                pais = PaisActual;
            }
            
            if (!string.IsNullOrEmpty(tabla) || !string.IsNullOrEmpty(codigo))
            {
                foreach (MensajeTO msg in mensajes)
                {
                    MensajeTO mensaje = msg.Clone() as MensajeTO;
                    if (!string.IsNullOrEmpty(msg.Tabla) && (msg.Tabla.Trim() == tabla.Trim())
                        && !string.IsNullOrEmpty(msg.Codigo) && msg.Codigo.Trim() == codigo.Trim()
                        && !string.IsNullOrEmpty(msg.Idioma) && msg.Idioma.Trim() == idioma.Trim()
                        && !string.IsNullOrEmpty(msg.Pais) && msg.Pais.Trim() == pais.Trim())
                    {
                        mensaje.Puntero = puntero;
                        mensaje.RazonTecnica = razonTecnica;
                        mensaje.Excepcion = excepcion;
                        mensaje.Campo = campo;
                        mensajesInstancia.Add(mensaje);
                        return mensaje;
                    }
                }

                if (tabla != TABLA_POR_DEFECTO_MENSAJES)
                {
                    return AgregarMensaje(TABLA_POR_DEFECTO_MENSAJES, codigo, campo, puntero, idioma, pais, excepcion, razonTecnica);
                }

                MensajeTO mensajeNulo = new MensajeTO()
                {
                    Tabla = TABLA_POR_DEFECTO_MENSAJES,
                    Codigo = codigo,
                    Campo = campo,
                    Glosa = string.Format("¡No se encuentra el mensaje en la tabla {0}, código {1} para el lenguaje {2}!", tabla, codigo, string.Format("{0}-{1}", idioma, pais)),
                    GlosaResumida = string.Format("¡{0}/{1}({2}) no encontrado!", tabla, codigo, string.Format("{0}-{1}", idioma, pais)),
                    Tipo = Severidad.Error,
                    Puntero = puntero,
                    RazonTecnica = razonTecnica,
                    Excepcion = excepcion
                };

                mensajesInstancia.Add(mensajeNulo);
                return mensajeNulo;
            }
            return null;
        }
        
        /// <summary>
        /// Comprueba la existencia de mensajes, según el nivel de severidad especificado.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <seealso cref="Ada.Framework.Data.Notifications.TO.Severidad"/>
        /// <param name="severidad">Nivel de severidad.</param>
        /// <returns><value>true</value> de existir alguno, o <value>false</value> de lo contrario.</returns>
        public virtual bool Hay(Severidad severidad)
        {
            return Mensajes.Count(x => x.Tipo == severidad) > 0;
        }

        /// <summary>
        /// Comprueba la existencia de mensajes, según el tipo de excepción especificado.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <typeparam name="T">Tipo excepción.</typeparam>
        /// <returns><value>true</value> de existir alguno, o <value>false</value> de lo contrario.</returns>
        public virtual bool HayExcepcionPorTipo<T>() where T : Exception
        {
            return mensajesInstancia.Count(c => c.Excepcion is T) > 0;
        }

        /// <summary>
        /// Obtiene los mensajes de base de datos y los vuelve a cargar en memoria.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static void Recargar()
        {
            Notificacion<IList<MensajeTO>> respuesta = new MensajeBO().ObtenerTodos();

            if(respuesta.HayError || respuesta.HayExcepcion)
            {
                throw new NotificacionesException(respuesta);
            }

            mensajes = respuesta.Respuesta;
        }

        /// <summary>
        /// Obtiene un mensaje según la tabla y el código recivido.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="tabla">Tabla de mensajes.</param>
        /// <param name="codigo">Código del mensaje.</param>
        /// <param name="idioma">Idioma del mensaje.</param>
        /// <param name="pais">Pais específico para el idioma del mensaje.</param>
        /// <returns>Representa un mensaje que se debe mostrar al usuario.</returns>
        public static MensajeTO ObtenerMensaje(string tabla, string codigo, string idioma = null, string pais = null)
        {
            if (tabla != null) tabla = tabla.Trim();
            if (codigo != null) codigo = codigo.Trim();
            if (idioma != null) idioma = idioma.Trim();
            if (pais != null) pais = pais.Trim();

            if (string.IsNullOrEmpty(tabla)) tabla = TABLA_POR_DEFECTO_MENSAJES;
            if (string.IsNullOrEmpty(idioma)) idioma = IdiomaActual;
            if (string.IsNullOrEmpty(pais)) pais = PaisActual;

            IEnumerable<MensajeTO> mensaje = mensajes.Where(c => c.Tabla == tabla && c.Codigo == codigo && c.Idioma == idioma && c.Pais == pais);

            if (mensaje.ToList().Count > 0)
            {
                return mensaje.Single();
            }
            else
            {
                if (tabla != TABLA_POR_DEFECTO_MENSAJES)
                {
                    return ObtenerMensaje(TABLA_POR_DEFECTO_MENSAJES, codigo);
                }

                MensajeTO retorno = new MensajeTO()
                {
                    Tabla = TABLA_POR_DEFECTO_MENSAJES,
                    Codigo = codigo,
                    Glosa = string.Format("¡No se encuentra el mensaje en la tabla {0}, código {1} para el lenguaje {2}!", tabla, codigo, string.Format("{0}-{1}", idioma, pais)),
                    GlosaResumida = string.Format("¡{0}/{1}({2}) no encontrado!", tabla, codigo, string.Format("{0}-{1}", idioma, pais)),
                    Tipo = Severidad.Error
                };

                return retorno;
            }
        }
        
        /// <summary>
        /// Agrega los mensajes de la instancia enviada a la actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="notificaciones">Colección de <see cref="Ada.Framework.Data.Notifications.Notificacion"/>Notificación.</param>
        public virtual void Unir(params Notificacion[] notificaciones)
        {
            Unir((notificaciones as IList<Notificacion>));
        }

        /// <summary>
        /// Agrega los mensajes de la instancia enviada a la actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="notificaciones">Colección de <see cref="Ada.Framework.Data.Notifications.Notificacion"/>Notificación.</param>
        public virtual void Unir(IList<Notificacion> notificaciones)
        {
            foreach (Notificacion notificacion in notificaciones)
            {
                if (notificacion != null)
                {
                    mensajesInstancia.Concat(notificacion.mensajesInstancia);
                }
            }
        }
    }
}