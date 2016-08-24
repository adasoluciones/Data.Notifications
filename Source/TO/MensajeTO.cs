using Ada.Framework.Core.CustomAttributes.Object;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ada.Framework.Data.Notifications.TO
{
    /// <summary>
    /// Representa un mensaje que se debe mostrar al usuario.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [Serializable]
    [Inmutable(true)]
    public class MensajeTO : ICloneable
    {
        /// <summary>
        /// Permite obtener el nombre de la tabla a la cual pertenece el mensaje.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Tabla { get; internal set; }

        /// <summary>
        /// Permite obtener el código único del mensaje.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Codigo { get; internal set; }

        /// <summary>
        /// Permite obtener el idioma del mensaje actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Idioma { get; internal set; }

        /// <summary>
        /// Permite obtener el país específico para el idioma del mensaje actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Pais { get; internal set; }

        /// <summary>
        /// Obtiene o establecer el nivel de severidad del mensaje.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public Severidad Tipo { get; internal set; }

        /// <summary>
        /// Obtiene el puntero a un fragmento del log (GUID).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Puntero { get; internal set; }

        /// <summary>
        /// Obtiene el campo al que esta asociado el mensaje.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Campo { get; internal set; }

        /// <summary>
        /// Permite obtener la excepción lanzada.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public Exception Excepcion { get; internal set; }

        /// <summary>
        /// Permite obtener un mensaje técnico con la razón de la excepción lanzada.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string RazonTecnica { get; internal set; }

        /// <summary>
        /// Obtiene la glosa completa a mostrar al usuario.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Glosa { get; internal set; }

        /// <summary>
        /// Obtiene la glosa resumida a mostrar al usuario.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string GlosaResumida { get; internal set; }

        /// <summary>
        /// Obtiene el enlace de audio con información o ayuda descriptiva al error.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string LinkAudio { get; internal set; }

        /// <summary>
        /// Obtiene un enlace de video con información o ayuda relacionada al error.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string LinkVideo { get; internal set; }

        /// <summary>
        /// Permite obtener los datos adicicionales de contexto del mensaje.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IDictionary<string, object> DatosAdicionales { get; internal set; }

        /// <summary>
        /// Constructor interno que inicializa la instacia.
        /// </summary>
        internal MensajeTO()
        {
            DatosAdicionales = new Dictionary<string, object>();
        }
                
        /// <summary>
        /// Copia en una nueva instancia el estado de la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Nueva instancia de MensajeTO.</returns>
        public object Clone()
        {
            MensajeTO retorno = new MensajeTO()
            {
                Tabla = Tabla,
                Codigo = Codigo,
                Idioma = Idioma,
                Pais = Pais,
                Tipo = Tipo,
                Campo = Campo,
                Puntero = Puntero,
                Excepcion = Excepcion,
                RazonTecnica = RazonTecnica,
                Glosa = Glosa,
                GlosaResumida = GlosaResumida,
                LinkAudio = LinkAudio,
                LinkVideo = LinkVideo
            };
            retorno.DatosAdicionales.Concat(DatosAdicionales);
            return retorno;
        }


        /// <summary>
        /// Valida los valores principales del objeto.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 15/06/2016 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>
        ///     Retorna <code>true</code> en caso de ser válido, o <code>false</code> de lo contrario.
        /// </returns>
        public virtual Notificacion<bool> Validar()
        {
            Notificacion<bool> retorno = new Notificacion<bool>();
            retorno.Respuesta = true;

            if (string.IsNullOrEmpty(Tabla))
            {
                retorno.AgregarMensaje(Notificacion.TABLA_POR_DEFECTO_MENSAJES, "ERROR_VACIO", "Tabla");
                retorno.Respuesta = false;
            }

            if (string.IsNullOrEmpty(Codigo))
            {
                retorno.AgregarMensaje(Notificacion.TABLA_POR_DEFECTO_MENSAJES, "ERROR_VACIO", "Codigo");
                retorno.Respuesta = false;
            }

            if (string.IsNullOrEmpty(Idioma))
            {
                retorno.AgregarMensaje(Notificacion.TABLA_POR_DEFECTO_MENSAJES, "ERROR_VACIO", "Idioma");
                retorno.Respuesta = false;
            }

            if (string.IsNullOrEmpty(Pais))
            {
                retorno.AgregarMensaje(Notificacion.TABLA_POR_DEFECTO_MENSAJES, "ERROR_VACIO", "Pais");
                retorno.Respuesta = false;
            }

            return retorno;
        }
    }
}
