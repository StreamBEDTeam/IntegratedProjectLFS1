var landingPageText = ["When water monitors assess stream habitats, "
                            + "they first decide what key features to focus on, "
                            + "then consider how these features compare to other habitats.",

                       "Your first task will be to choose one image "
                            + "that best presents each area and feature.<br/><br/>"
                            + "Then, you will compare each selected image to a set "
                            + "of reference images.",
                       
                       "Task 2 minutes to familiarize yourself with the key areas "
                            + "of the stream habitat.<br/><br/>"
                            + "When you are ready, select Area 1, and select 1 image that "
                            + "best represents each feature."];

var i = 0;

function nextButtonOnClick() {
    if (i < 3) {
        var element = document.getElementById("pageContent");

        element.classList.remove("fadeInTransition");

        void element.offsetWidth;

        element.innerHTML = landingPageText[i];
        element.classList.add("fadeInTransition");

        i++;
    }
    else {
        window.location.href = "ms-appx-web:///views/areas.html";
    }
}

window.onload = function() {
    i = 0;

    document.getElementById("pageContent").innerHTML = landingPageText[i];
    i++;
}