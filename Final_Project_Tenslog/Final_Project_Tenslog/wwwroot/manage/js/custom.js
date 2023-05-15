$(document).ready(function () {
    console.log("salam")
    $('#userSearchAdmin').on('keyup', function () {
        console.log($(this).val())
        fetch('User/SearchUser?search=' + $(this).val())
            .then(res => {
                return res.text();
            })
            .then(data => {
                $('.adminPanelUserSearch').html(data)
            })
    })
})