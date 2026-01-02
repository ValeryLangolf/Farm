public class NextPageButton : PageScrollButtonBase
{
    protected override void OnClick() =>
        PagedContainer.IncreasePage();

    protected override void OnPageChanged(int currentPage, int totalPages)
    {
        bool shouldBeActive = totalPages > 1 && currentPage < totalPages - 1;
        SetActiveButton(shouldBeActive);
    }
}