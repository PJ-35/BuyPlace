//window.typewriterEffect = {
//    init: function (elementId, options) {
//        const element = document.getElementById(elementId);
//        if (element) {
//            new Typewriter(element, options);
//        }
//    }
//};

window.typewriterEffect = {
    init: function (elementId, options) {
        const element = document.getElementById(elementId);
        if (element) {
            const existingText = element.innerText;

            // Effacez le texte existant pour permettre à Typewriter de le redéfiler
            element.innerText = '';

            // Initialisez Typewriter avec le texte existant
            const typewriter = new Typewriter(element, options);
            typewriter.typeString(existingText).start();
        }
    }
};