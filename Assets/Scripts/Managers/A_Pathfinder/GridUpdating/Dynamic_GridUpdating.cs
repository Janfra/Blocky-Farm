
using System;

public class Dynamic_GridUpdating : Static_GridUpdating
{
    public static event Action<IGridUpdatable> OnDynamicUpdateGrid;

    private void Awake()
    {
        Init();
    }
}
