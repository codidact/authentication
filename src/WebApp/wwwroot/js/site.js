const $ = (selector) => document.querySelector(selector);
/*
 * Header slides
 ----------------
 * This code powers the "header slides", which are at the core of mobile nav
 */

const headerSlideTriggers = document.querySelectorAll(
    "[data-trigger-header-slide]"
);

for (let i = 0; i < headerSlideTriggers.length; i++) {
    headerSlideTriggers[i].addEventListener("click", function (e) {
        const headerSlide = document.querySelector(
            this.getAttribute("data-trigger-header-slide")
        );

        headerSlide.classList.toggle("is-active");
        this.classList.toggle("is-active");

        // Position header slide appropriately relative to
        // trigger.
        const rect = this.getBoundingClientRect();
        headerSlide.style.top = rect.top + rect.height + "px";
        headerSlide.style.right = document.body.clientWidth - rect.right + "px";

        // Prevent navigation
        e.preventDefault();
    });
}
/*
 * Email verification
 ---------------------
 * This code sends a POST request that send an email to the user
 * for verifying their email
 */
$(".js-email-verify").addEventListener("click", () => {
    $(".js-email-verify").setAttribute("disabled", "true");
    // temproray, will remove when co-design implements it
    $(".js-email-verify").style.opacity = 0.7;
    fetch("/account/email-verification?handler=emailVerify", {
            method: "POST",
            headers: new Headers({
                RequestVerificationToken: $(
                    'input[name="__RequestVerificationToken"]'
                ).value,
            }),
        })
        .then((obj) => obj.text())
        .then((response) => {
            $(".js-email-verify").removeAttribute("disabled");
            $(".js-email-verify").style.opacity = 1;
            if (response === "Email-Sent") {
                // TODO: Add a loading bar here.
                $(".js-email-verification-status").innerText =
                    "Sent! Check your inbox";
            } else {
                $(".js-email-verification-status").innerText =
                    "There has been a problem, please try again";
            }
        });
});
