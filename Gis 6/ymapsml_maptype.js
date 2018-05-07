ymaps.ready(init);

function init () {
    // Создание карты.
    var mapSatellite = new ymaps.Map('satelliteTypeId', {
            center: [55.790881, 49.121779],
            zoom: 14
        }, {
            searchControlProvider: 'yandex#search'
        });

    ymaps.geoXml.load('data.xml')
        .then(function (res) {
            if (res.mapState) {
                // Изменение типа карты.
                res.mapState.applyToMap(mapSatellite);
            }
        },
        // Вызывается в случае неудачной загрузки YMapsML-файла.
        function (error) {
            alert('При загрузке YMapsML-файла произошла ошибка: ' + error);
        });
}