$(document).ready(function () {
    $('.sharePost').on('click', function (e) {
        e.preventDefault();
        var link = $(this).attr('href');
        var rootLink = window.location.origin;
        var url = rootLink + link;
        console.log(url);

        var tempInput = document.createElement('input');
        tempInput.setAttribute('value', url);
        document.body.appendChild(tempInput);
        tempInput.select();
        document.execCommand('copy');
        document.body.removeChild(tempInput);
        Swal.fire({
            text: "Link kopyalandı!",
            icon: "success",
            timer: 1500,
            showConfirmButton: false
        });
    });

    $('#searchUserInput').on('keyup', function () {
        console.log($(this).val())
        if ($(this).val() != "") {
            console.log($(this).val())
            $('.reelsContent').addClass('d-none')
            $('.searchUser').removeClass('d-none')

            fetch('/Reels/SearchUser?search=' + $(this).val())
                .then(res => {
                    return res.text();
                })
                .then(data => {
                    $('.searchUser').html(data)
                })

        }
        else {
            $('.reelsContent').removeClass('d-none')
            $('.searchUser').addClass('d-none')
        }
    })
    $('#searchMobileUserInput').on('keyup', function () {
        console.log($(this).val())
        if ($(this).val() != "") {
            console.log($(this).val())
            $('.reelsMobileConten').addClass('d-none')
            $('.searchUser').removeClass('d-none')

            fetch('/Reels/SearchUser?search=' + $(this).val())
                .then(res => {
                    return res.text();
                })
                .then(data => {
                    $('.searchUser').html(data)
                })

        }
        else {
            $('.reelsMobileConten').removeClass('d-none')
            $('.searchUser').addClass('d-none')
        }
    })
    $('.saveBtn').on('click', function () {
        if ($('.saveBtn').hasClass('fa-solid')) {
            $('.saveBtn').addClass('fa-regular')
            $('.saveBtn').removeClass('fa-solid')
        }
        else {
            $('.saveBtn').addClass('fa-solid')
            $('.saveBtn').removeClass('fa-regular')
        }
    })
    $('.likersCommentsBtn').on('click', function (e) {
        e.preventDefault()
        if ($('.commentsArea').hasClass('d-none')) {
            $('.likersArea').addClass('d-none')
            $('.commentsArea').removeClass('d-none')
        }
        else {
            $('.commentsArea').addClass('d-none')
            $('.likersArea').removeClass('d-none')
        }
    })


    $('.myPostsProfile').on('click', function (e) {
        e.preventDefault();
        $('.myProfileTabs').addClass('d-none');
        $('.myProfilePosts').removeClass('d-none');
    })
    $('.followersTab').on('click', function (e) {
        e.preventDefault();
        $('.myProfileTabs').addClass('d-none');
        $('.myProfileFollowers').removeClass('d-none');
    })
    $('.followingsTab').on('click', function (e) {
        e.preventDefault();
        $('.myProfileTabs').addClass('d-none');
        $('.myProfileFollowings').removeClass('d-none');
    })
    $('.mySavedProfile').on('click', function (e) {
        e.preventDefault();
        $('.myProfileTabs').addClass('d-none');
        $('.myProfileSaveds').removeClass('d-none');
    })


    $('.edit').on('click', function (e) {
        e.preventDefault()
        if ($('.editProfile').hasClass('d-none')) {
            $('.editProfile').removeClass('d-none')
        } else {
            $('.editProfile').addClass('d-none')
        }
    })
    $('.addLikePost').on('click', function (e) {
        e.preventDefault()
        var id = $(this).data('id');
        fetch('/post/like/' + id)
            .then(res => {
                return res.json();
            })
            .then(data => {
                $('.postLikeCount' + id).text(data);
            });
    })
    $('.addSavedPost').on('click', function (e) {
        e.preventDefault()
        var id = $(this).data('id');
        fetch('/post/save/' + id)
            .then(res => {
                return res.json();
            })
            .then(data => {
                throw data;
            });
    })
    $('.verificationBtn').on('click', function (e) {
        e.preventDefault()
        if ($('.verification').hasClass('d-none')) {
            $('.verification').removeClass('d-none')
        } else {
            $('.verification').addClass('d-none')
        }
    })

    $('.privacyBtn').on('click', function (e) {
        e.preventDefault()
        if ($('.privacy').hasClass('d-none')) {
            $('.privacy').removeClass('d-none')
        } else {
            $('.privacy').addClass('d-none')
        }
    })

    $('.helpBtn').on('click', function (e) {
        e.preventDefault()
        if ($('.helpTab').hasClass('d-none')) {
            $('.helpTab').removeClass('d-none')
        } else {
            $('.helpTab').addClass('d-none')
        }
    })

    $('.changePass').on('click', function (e) {
        e.preventDefault()
        if ($('.changePassword').hasClass('d-none')) {
            $('.changePassword').removeClass('d-none')
        } else {
            $('.changePassword').addClass('d-none')
        }
    })

    $('.videoCall').on('click', function () {
        $('.chatMainPage').addClass('d-none')
        $('.callPage').removeClass('d-none')
    })
    $('.voiceCall').on('click', function () {
        $('.chatMainPage').addClass('d-none')
        $('.callPage').removeClass('d-none')
    })

    $('.endCall').on('click', function () {
        $('.chatMainPage').removeClass('d-none')
        $('.callPage').addClass('d-none')
    })

    $('.act').on('click', function (e) {
        e.preventDefault()
        $('.act').removeClass('activeForSettings')
        $(this).addClass('activeForSettings')

        if ($(this).hasClass('act1')) {
            $('.tabSetting').addClass('d-none')
            $('.editProfile').removeClass('d-none')
        }
        if ($(this).hasClass('act2')) {
            $('.tabSetting').addClass('d-none')
            $('.changePassword').removeClass('d-none')
        }
        if ($(this).hasClass('act3')) {
            $('.tabSetting').addClass('d-none')
            $('.securityAndActivity').removeClass('d-none')
        }
        if ($(this).hasClass('act5')) {
            $('.tabSetting').addClass('d-none')
            $('.verification').removeClass('d-none')
        }
        if ($(this).hasClass('act4')) {
            $('.tabSetting').addClass('d-none')
            $('.help').removeClass('d-none')
        }
    })

    $(".startCall").on('click', function () {
        $('.chat-modal').addClass('d-none')
        $('.call-container').removeClass('d-none')
    })
    $(".endCall").on('click', function () {
        $('.chat-modal').removeClass('d-none')
        $('.call-container').addClass('d-none')
    })
    $(".chat-list a").click(function () {
        $(".chatbox").addClass('showbox');
        return false;
    });

    $(".chat-icon").click(function () {
        $(".chatbox").removeClass('showbox');
    });
    $('.like').on('click', function () {
        $(this).toggleClass('fa-regular fa-solid');
        if ($(this).hasClass('fa-solid')) {
            $(this).addClass('activeForLike');
        } else {
            $(this).removeClass('activeForLike');
        }
    });
    $('.savePost').on('click', function () {
        $(this).toggleClass('fa-regular fa-solid');
    });
    $('.navItems').on('click', function (e) {
        $('.navItems').removeClass('active')
        $(this).addClass('active');
    })
    $('#settings').on('click', function () {
        if ($('#settingTable').hasClass('d-none')) {
            $('#settingTable').removeClass('d-none');
        } else {
            $('#settingTable').addClass('d-none');
        }
    });
    $(document).on('click', function (e) {
        if (!$('.navigation, #settings').has(e.target).length) {
            $('#settingTable').addClass('d-none');
        }
    });
    $("#myModal").modal('show');
    function tabChange() {
        var tabs = $('.nav-tabs > li');
        var active = tabs.filter('.active');
        var next = active.next('li').length ? active.next('li').find('a') : tabs.filter(':first-child').find('a');
        next.tab('show');
    }

    $('.tab-pane').hover(function () {
        clearInterval(tabCycle);
    }, function () {
        tabCycle = setInterval(tabChange, 5000);
    });
    var tabCycle = setInterval(tabChange, 5000)
    $(function () {
        $('.nav-tabs a').click(function (e) {
            clearInterval(tabCycle);
            $(this).tab('show')
            tabCycle = setInterval(tabChange, 5000);
        });
    });
});
document.querySelector(".nav-item").focus();
function openNav() {
    document.getElementById("mySidenav").style.width = "400px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}
