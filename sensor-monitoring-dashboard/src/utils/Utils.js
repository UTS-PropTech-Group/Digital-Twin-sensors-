function padTo2Digits(num) {
  return num.toString().padStart(2, '0');
}

export function formatDate(date) {
  return (
    [
      date.getFullYear(),
      padTo2Digits(date.getMonth() + 1),
      padTo2Digits(date.getDate()),
    ].join('-') +
    ' ' +
    [
      padTo2Digits(date.getHours()),
      padTo2Digits(date.getMinutes()),
      padTo2Digits(date.getSeconds()),
    ].join(':')
  );
}

export function getCursorPosition(canvas, event) {
  const rect = canvas.getBoundingClientRect()
  const x = event.clientX - rect.left
  const y = event.clientY - rect.top
  console.log("x: " + x + " y: " + y)
}

export function getRandomColor() {
  var trans = '0.5'; // 50% transparency
  var color = 'rgba(';
  for (var i = 0; i < 3; i++) {
    color += Math.floor(Math.random() * 255) + ',';
  }
  color += trans + ')'; // add the transparency
  return color;
}

export function heatMapColorforValue(value) {
  var h = (1.0 - value) * 240
  return "hsla(" + h + ", 100%, 50%, 70%)";
}

export function downloadAsCsv(document, headers, data, fileName) {
  var csv = headers.join(',') + '\n';

  //merge the data with CSV  
  data.forEach(function (record) {
    let keys = Object.keys(record)
    let row = []
    for(let k of keys){
      row.push(record[k])
    }
    csv += row.join(',');
    csv += "\n";
  });

  var hiddenElement = document.createElement('a');
  hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csv);
  hiddenElement.target = '_blank';

  //provide the name for the CSV file to be downloaded  
  hiddenElement.download = fileName;
  hiddenElement.click();
}