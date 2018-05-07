// Обратите внимание, что модуль не умеет обсчитывать полигоны с самопересечениями.
ymaps.ready(['util.calculateArea']).then(function () {
    var myMap = new ymaps.Map("map", {
            center: [55.790881, 49.121779],
            zoom: 16,
            controls: ['searchControl', 'zoomControl']
        }, {
            searchControlProvider: 'yandex#search'
        }),
    // Создаем многоугольник, круг и прямоугольник.
        polygon = new ymaps.GeoObject({
            geometry: {
                type: "Polygon", coordinates: [[
                    [55.791961, 49.119698],
                    [55.790570, 49.123077],
                    [55.789936, 49.122970],
                    [55.788811, 49.121543],
                    [55.790576, 49.117842]
                ]]
            }
        }),
        collection = new ymaps.GeoObjectCollection();
    // Добавляем геообъекты в коллекцию.
    collection.add(polygon);
    // Добавляем коллекцию на карту.
    myMap.geoObjects.add(collection);

    collection.each(function (obj) {
        // Вычисляем площадь геообъекта.
        var area = Math.round(ymaps.util.calculateArea(obj)),
        // Вычисляем центр для добавления метки.
            center = ymaps.util.bounds.getCenter(obj.geometry.getBounds());
        // Если площадь превышает 1 000 000 м², то приводим ее к км².
        if (area <= 1e6) {
            area += ' м²';
        } else {
            area = (area / 1e6).toFixed(3) + ' км²';
        }
        obj.properties.set('balloonContent', area);

        myMap.geoObjects.add(new ymaps.Placemark(center, {'iconCaption': area}, {preset: 'islands#greenDotIconWithCaption'}));
    });
});
