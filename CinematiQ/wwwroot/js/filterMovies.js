function goToPage(pageNumber) {
    console.log('������������ �� ��������', pageNumber);
}
document.querySelectorAll('.page-number').forEach(function (button) {
    button.addEventListener('click', function () {
        var pageNumber = this.textContent;
        goToPage(pageNumber);
    });
});
document.getElementById('prev-page').addEventListener('click', function () {
    console.log('���������� ��������');
    //  �� ���������� ��������
});

document.getElementById('next-page').addEventListener('click', function () {
    console.log('��������� ��������');
    //  �� ��������� ��������
});
