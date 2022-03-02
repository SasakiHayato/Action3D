
/// <summary>
/// Singletonにするクラスに継承するクラス
/// </summary>
/// <typeparam name="Singleton">派生クラス</typeparam>

public class SingletonAttribute<Singleton> where Singleton : class
{
    Singleton _sigleton = default;
    public static Singleton Instance => s_instance._sigleton;

    private static SingletonAttribute<Singleton> s_instance = null;
    public static void SetInstance(Singleton singleton, SingletonAttribute<Singleton> attribute)
    {
        if (s_instance == null)
        {
            s_instance = new SingletonAttribute<Singleton>();
            s_instance._sigleton = singleton;
            attribute.SetUp();
        }
    }

    public virtual void SetUp() { }
    public virtual void Destory() { s_instance = null; }
}
