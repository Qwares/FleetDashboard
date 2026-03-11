// TelemaSense report export: CSV download and simple PDF via jsPDF
window.fleetReportExport = {
  downloadCsv: function (csvContent, filename) {
    try {
      const blob = new Blob(['\uFEFF' + (csvContent || '')], { type: 'text/csv;charset=utf-8;' });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = filename || 'telemasense-raporu.csv';
      a.click();
      URL.revokeObjectURL(url);
    } catch (e) { console.error('CSV export error:', e); }
  },

  downloadPdf: function (title, rows, unit) {
    try {
      var JsPDF = (window.jspdf && (window.jspdf.jsPDF || window.jspdf)) || (window.jspdf && window.jspdf.default && (window.jspdf.default.jsPDF || window.jspdf.default)) || (window['jspdf'] && (window['jspdf'].jsPDF || window['jspdf']));
      if (!JsPDF) {
        console.error('jspdf not loaded');
        return;
      }
      const doc = new JsPDF({ orientation: 'portrait', unit: 'mm', format: 'a4' });
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
      const row = [r.plate || '', String(r.value ?? ''), String(r.threshold ?? ''), r.isNightShift ? 'Evet' : 'Hayır', (r.policyStatus === 'Danger' ? 'Tehlikeli' : 'Güvenli')];
      row.forEach((cell, i) => {
        doc.text(String(cell).substring(0, 22), x + 2, y + 5);
        x += colWidths[i];
      });
      y += 8;
    });

    doc.save('telemasense-raporu.pdf');
    } catch (e) { console.error('PDF export error:', e); }
  },

  exportChartToPng: function () {
    try {
      const el = document.getElementById('fleet-chart-export');
      if (!el) { console.error('Chart element #fleet-chart-export not found'); return; }
      var capture = window.html2canvas || (typeof html2canvas !== 'undefined' && html2canvas);
      if (!capture) { console.error('html2canvas not loaded'); return; }
      capture(el, { useCORS: true, allowTaint: true, scale: 2, logging: false }).then(function (canvas) {
        const link = document.createElement('a');
        link.download = 'telemasense-grafik-' + new Date().toISOString().slice(0, 10) + '.png';
        link.href = canvas.toDataURL('image/png');
        link.click();
      }).catch(function (err) { console.error('PNG export error:', err); });
    } catch (e) { console.error('PNG export error:', e); }
  }
};
