const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');

signUpButton.addEventListener('click', () =>
    container.classList.add('right-panel-active'));

signInButton.addEventListener('click', () =>
    container.classList.remove('right-panel-active'));  

    function goSignIn(e){
        document.getElementById('logInMobile').style.display = "none";
        document.getElementById('signInMobile').style.display = "block";

    }
    function goLogIn(e){
        document.getElementById('logInMobile').style.display = "block";
        document.getElementById('signInMobile').style.display = "none";
    }