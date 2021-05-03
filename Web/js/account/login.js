var video = new Video("backgroundVideo");
video.enableAutoplay();
video.enableLoop();

video.enableAudio();
video.setVolume(0.2);
document.getElementById("audioControlTitle").className = "fa fa-volume-up"

function switchAudioState() {
    if (video.getAudioState() === "enable") {
        video.disableAudio();
        document.getElementById("audioControlTitle").className = "fa fa-volume-mute"
    } else {
        video.enableAudio();
        video.setVolume(0.2);
        document.getElementById("audioControlTitle").className = "fa fa-volume-up";
    }
}

setTimeout(function (e) {
    var path = window.location.pathname;
    if (path == "/account/login/login-success")
        location.replace(window.location.protocol + "//" + window.location.hostname + "/home");
    if (path === "/account/login/login-failed_not-exists")
        location.replace(window.location.protocol + "//" + window.location.hostname + "/account/register");
}, 3000);