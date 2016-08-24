using Ada.Framework.Core;

namespace Ada.Framework.Data.Notifications.TO
{
    /// <summary>
    /// Representa el nivel de severidad de los mensajes.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public class Severidad : Enumeracion<string>
    {
        /// <summary>
        /// Constructor interno (protected) que permite definir tipos de Severidad. 
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="codigo">Código real del estado.</param>
        protected Severidad(string codigo) : base(codigo) { }

        /// <summary>
        /// Representa una advertencia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static Severidad Alert = new Severidad("Alert");

        /// <summary>
        /// Representa un error grave, pero que no impide continiuar.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static Severidad Error = new Severidad("Error");

        /// <summary>
        /// Representa un error que impide continiuar.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static Severidad Fatal = new Severidad("Fatal");

        /// <summary>
        /// Representa una información.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static Severidad Info = new Severidad("Info");

        /// <summary>
        /// Representa un mensaje de éxito.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public static Severidad Success = new Severidad("Success");
    }
}
