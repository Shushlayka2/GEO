ymaps.ready(init);

function init () {
    // Создание экземпляра карты.
    var myMap = new ymaps.Map('map', {
            center: [55.790881, 49.121779],
            zoom: 16
        }, {
            searchControlProvider: 'yandex#search'
        });

    // Загрузка YMapsML-файла.
    ymaps.geoXml.load('data.xml')
        .then(function (res) {
            // Добавление геообъектов на карту.
            myMap.geoObjects.add(res.geoObjects);
           // Вызывается в случае неудачной загрузки YMapsML-файла.
        }, function (error){
            alert('Ошибка: ' + error);
        });
}