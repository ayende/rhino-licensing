namespace Rhino.Licensing.AdminTool.Services
{
    public interface IStatusService
    {
        /// <summary>
        /// Shows a status message on status bar
        /// </summary>
        /// <param name="message"></param>
        void Update(string message);

        /// <summary>
        /// Shows a status message on status bar
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Update(string message, params object[] args);
    }
}