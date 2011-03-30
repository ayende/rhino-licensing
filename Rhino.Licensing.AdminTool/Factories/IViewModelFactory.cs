using Caliburn.PresentationFramework.Screens;

namespace Rhino.Licensing.AdminTool.Factories
{
    public interface IViewModelFactory
    {
        /// <summary>
        /// Creates a new instance of 
        /// specified view model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>() where T : IScreen;

        /// <summary>
        /// Releases a view model and release 
        /// it for garbage collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        void Release(IScreen viewModel);
    }
}