using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI;

public interface IModelInterceptor
{
    IModelContext Intercept(IModelContext modelContext);
}

public interface IModelContext
{
    object NewModel { get; }

    IResourceContext ResourceContext { get; }
}

public class ModelContext : IModelContext
{
    public ModelContext(object newModel, IResourceContext resourceContext)
    {
        NewModel = newModel;
        ResourceContext = resourceContext;
    }

    public object NewModel { get; }

    public IResourceContext ResourceContext { get; }
}