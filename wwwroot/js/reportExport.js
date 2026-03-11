// TelemaSense report export: CSV download and simple PDF via jsPDF
window.fleetReportExport = {
  downloadCsv: function (csvContent, filename) {
    const blob = new Blob(['\uFEFF' + csvContent], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename || 'telemasense-raporu.csv';
    a.click();
    URL.revokeObjectURL(url);
  },

  downloadPdf: function (title, rows, unit) {
    const jspdfLib = window.jspdf;
    if (!jspdfLib || !jspdfLib.jsPDF) {
      console.error('jspdf not loaded');
      return;
    }
    const doc = new jspdfLib.jsPDF({ orientation: 'portrait', unit: 'mm', format: 'a4' });
    const pageW = doc.internal.pageSize.getWidth();
    let y = 15;
    const colWidths = [45, 35, 30, 40, 40];
    const headers = ['Plaka', 'Değer (' + (unit || 'km/s') + ')', 'Eşik', 'Gece Vardiyası', 'Politika'];

    doc.setFontSize(14);
    doc.text(title || 'Filo Raporu', 14, y);
    y += 10;
    doc.setFontSize(10);

    doc.setFillColor(30, 42, 58);
    doc.rect(14, y, pageW - 28, 8, 'F');
    doc.setTextColor(255, 255, 255);
    doc.setFont(undefined, 'bold');
    let x = 14;
    headers.forEach((h, i) => {
      doc.text(h, x + 2, y + 5.5);
      x += colWidths[i];
    });
    y += 10;
    doc.setTextColor(0, 0, 0);
    doc.setFont(undefined, 'normal');

    (rows || []).forEach(function (r, idx) {
      if (y > 270) {
        doc.addPage();
        y = 15;
      }
      x = 14;
      const row = [r.plate || '', String(r.value ?? ''), String(r.threshold ?? ''), r.isNightShift ? 'Evet' : 'Hayır', r.policyStatus === 'Danger' ? 'Tehlikeli' : 'Güvenli'];
      row.forEach((cell, i) => {
        doc.text(String(cell).substring(0, 22), x + 2, y + 5);
        x += colWidths[i];
      });
      y += 8;
    });

    doc.save('telemasense-raporu.pdf');
  },

  exportChartToPng: function () {
    const el = document.getElementById('fleet-chart-export');
    if (!el) return;
    if (typeof html2canvas !== 'undefined') {
      html2canvas(el, { useCORS: true, scale: 2, logging: false }).then(function (canvas) {
        const link = document.createElement('a');
        link.download = 'telemasense-grafik-' + new Date().toISOString().slice(0, 10) + '.png';
        link.href = canvas.toDataURL('image/png');
        link.click();
      });
    }
  }
};
