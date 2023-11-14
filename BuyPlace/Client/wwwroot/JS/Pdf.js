function generatePDF() {
    // Options de configuration pour html2pdf
    var options = {
        margin: 10,
        filename: 'facture.pdf',
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };

    // �l�ments � inclure dans le PDF (ici, la modal-body)
    var element = document.querySelector('.modal-body');

    // G�n�rer le PDF
    html2pdf(element, options);
}