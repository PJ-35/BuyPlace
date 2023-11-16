function Print() {
    // Options de configuration pour html2pdf
    var myElement = document.getElementById("facturePrint")

    var contenuOriginal = document.body.innerHTML;
    document.body.innerHTML = myElement.innerHTML;


    window.print();
   // document.body.innerHTML = contenuOriginal;
    window.location.reload()

    //var zoneImprimer = document.getElementById("facturePrint").outerHTML;


    //var nouvelleFenetre = window.open('', '_blank');
    //nouvelleFenetre.document.write(zoneImprimer)

    //nouvelleFenetre.document.write('<html><head><title>Impression</title></head><body>');

    //nouvelleFenetre.document.write(zoneImprimer);

    //nouvelleFenetre.document.write('</body></html>');

    //nouvelleFenetre.document.close();


    //nouvelleFenetre.print();

}