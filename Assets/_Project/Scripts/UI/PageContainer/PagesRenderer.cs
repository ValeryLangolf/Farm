using System.Collections.Generic;

public class PagesRenderer
{
    private readonly ItemsCollection _itemsCollection;
    private readonly PageNavigation _pageNavigation;

    public PagesRenderer(ItemsCollection itemsCollection, PageNavigation pageNavigation)
    {
        _itemsCollection = itemsCollection;
        _pageNavigation = pageNavigation;
    }

    public void UpdateView()
    {
        int startIndex = _pageNavigation.GetCurrentStartIndex();
        int takeCount = _pageNavigation.GetCurrentTakeCount();
        _itemsCollection.EnableItemsInRange(startIndex, takeCount);

        UpdateItemsOrder(startIndex, takeCount);
    }

    private void UpdateItemsOrder(int startIndex, int takeCount)
    {
        IReadOnlyList<IPagedItem> itemsInCurrentPage = _itemsCollection.GetItemsInPageRange(startIndex, takeCount);

        if (itemsInCurrentPage.Count == 0)
            return;

        for (int index = 0; index < itemsInCurrentPage.Count; index++)
        {
            int siblingIndex = (itemsInCurrentPage.Count - 1) - index;
            itemsInCurrentPage[index].GameObject.transform.SetSiblingIndex(siblingIndex);
        }
    }
}