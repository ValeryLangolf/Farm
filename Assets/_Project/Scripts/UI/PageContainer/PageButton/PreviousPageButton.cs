public class PreviousPageButton : PageScrollButtonBase
{
    protected override void OnClick() =>
        PagedContainer.DecreasePage();

    protected override void OnPageChanged(int currentPage, int totalPages)
    {
        bool shouldBeActive = totalPages > 1 && currentPage > 0;
        SetActiveButton(shouldBeActive);
    }
}