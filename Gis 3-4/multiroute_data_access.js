function init () {
    // Создаем модель мультимаршрута.
    var multiRouteModel = new ymaps.multiRouter.MultiRouteModel([
            [55.760854, 49.304490], [55.786795, 49.121449], [55.791790, 49.119375], [55.793177, 49.149309], [55.786795, 49.121449]
        ], {
            // Путевые точки можно перетаскивать.
            // Маршрут при этом будет перестраиваться.
            wayPointDraggable: true,
            boundsAutoApply: true
        }),

        // Создаём выпадающий список для выбора типа маршрута.
        routeTypeSelector = new ymaps.control.ListBox({
            data: {
                content: 'Как добраться'
            },
            items: [
                new ymaps.control.ListBoxItem({data: {content: "Авто"},state: {selected: true}}),
                new ymaps.control.ListBoxItem({data: {content: "Общественным транспортом"}}),
                new ymaps.control.ListBoxItem({data: {content: "Пешком"}})
            ],
            options: {
                itemSelectOnClick: false
            }
        }),
        // Получаем прямые ссылки на пункты списка.
        autoRouteItem = routeTypeSelector.get(0),
        masstransitRouteItem = routeTypeSelector.get(1),
        pedestrianRouteItem = routeTypeSelector.get(2);

    // Подписываемся на события нажатия на пункты выпадающего списка.
    autoRouteItem.events.add('click', function (e) { changeRoutingMode('auto', e.get('target')); });
    masstransitRouteItem.events.add('click', function (e) { changeRoutingMode('masstransit', e.get('target')); });
    pedestrianRouteItem.events.add('click', function (e) { changeRoutingMode('pedestrian', e.get('target')); });

    ymaps.modules.require([
        'MultiRouteCustomView'
    ], function (MultiRouteCustomView) {
        // Создаем экземпляр текстового отображения модели мультимаршрута.
        // см. файл custom_view.js
        new MultiRouteCustomView(multiRouteModel);
    });

    // Создаем карту с добавленной на нее кнопкой.
    var myMap = new ymaps.Map('map', {
            center: [55.790881, 49.121779],
            zoom: 7,
            controls: [routeTypeSelector]
        }, {
            buttonMaxWidth: 300
        }),

        // При клике на метке будет открываться балун,
        // содержащий Яндекс.Панораму в текущей географической точке.
        //КФУ
        myPlacemark1 = new ymaps.Placemark([55.791790, 49.119375], {
            // Для данной метки нужно будет открыть воздушную панораму.
            panoLayer: 'yandex#panorama'
        }, {
            preset: 'islands#nightIcon',
            openEmptyBalloon: true,
            balloonPanelMaxMapArea: 0
        }),

        //Корстон
        myPlacemark2 = new ymaps.Placemark([55.793177, 49.149309], {
            // Для этой метки будем запрашивать наземную панораму.
            panoLayer: 'yandex#panorama'
        }, {
            preset: 'islands#nightIcon',
            openEmptyBalloon: true,
            balloonPanelMaxMapArea: 0
        });

        //Мой дом
        myPlacemark3 = new ymaps.Placemark([55.771385, 49.152701], {
            // Для этой метки будем запрашивать наземную панораму.
            panoLayer: 'yandex#panorama'
        }, {
            preset: 'islands#nightIcon',
            openEmptyBalloon: true,
            balloonPanelMaxMapArea: 0
        });

        //Отель
        myPlacemark4 = new ymaps.Placemark([55.787152, 49.122009], {
            // Для этой метки будем запрашивать наземную панораму.
            panoLayer: 'yandex#panorama'
        }, {
            preset: 'islands#nightIcon',
            openEmptyBalloon: true,
            balloonPanelMaxMapArea: 0
        });

         // Функция, устанавливающая для метки макет содержимого ее балуна.
    function setBalloonContentLayout (placemark, panorama) {
        // Создание макета содержимого балуна.
        var BalloonContentLayout = ymaps.templateLayoutFactory.createClass(
            '<div id="panorama" style="width:256px;height:156px"></div>', {
                // Переопределяем функцию build, чтобы при формировании макета
                // создавать в нем плеер панорам.
                build: function () {
                    // Сначала вызываем метод build родительского класса.
                    BalloonContentLayout.superclass.build.call(this);
                    // Добавляем плеер панорам в содержимое балуна.
                    this._openPanorama();
                },
                // Аналогично переопределяем функцию clear, чтобы удалять
                // плеер панорам при удалении макета с карты.
                clear: function () {
                    this._destroyPanoramaPlayer();
                    BalloonContentLayout.superclass.clear.call(this);
                },
                // Добавление плеера панорам.
                _openPanorama: function () {
                    if (!this._panoramaPlayer) {
                        // Получаем контейнер, в котором будет размещаться наша панорама.
                        var el = this.getParentElement().querySelector('#panorama');
                        this._panoramaPlayer = new ymaps.panorama.Player(el, panorama, {
                            controls: ['panoramaName']
                        });
                    }
                },
                // Удаление плеера панорамы.
                _destroyPanoramaPlayer: function () {
                    if (this._panoramaPlayer) {
                        this._panoramaPlayer.destroy();
                        this._panoramaPlayer = null;
                    }
                }
            });
        // Устанавливаем созданный макет в опции метки.
        placemark.options.set('balloonContentLayout', BalloonContentLayout);
    }

    // В этой функции выполняем проверку на наличие панорамы в данной точке.
    // Если панорама нашлась, то устанавливаем для балуна макет с этой панорамой,
    // в противном случае задаем для балуна простое текстовое содержимое.
    function requestForPanorama (e) {
        var placemark = e.get('target'),
            // Координаты точки, для которой будем запрашивать панораму.
            coords = placemark.geometry.getCoordinates(),
            // Тип панорамы (воздушная или наземная).
            panoLayer = placemark.properties.get('panoLayer');

        placemark.properties.set('balloonContent', "Идет проверка на наличие панорамы...");

        // Запрашиваем объект панорамы.
        ymaps.panorama.locate(coords, {
            layer: panoLayer
        }).then(
            function (panoramas) {
                if (panoramas.length) {
                    // Устанавливаем для балуна макет, содержащий найденную панораму.
                    setBalloonContentLayout(placemark, panoramas[0]);
                } else {
                    // Если панорам не нашлось, задаем
                    // в содержимом балуна простой текст.
                    placemark.properties.set('balloonContent', "Для данной точки панорамы нет.");
                }
            },
            function (err) {
                placemark.properties.set('balloonContent',
                    "При попытке открыть панораму произошла ошибка: " + err.toString());
            }
        );
    }

    // Слушаем на метках событие 'balloonopen': как только балун будет впервые открыт,
    // выполняем проверку на наличие панорамы в данной точке и в случае успеха создаем
    // макет с найденной панорамой.
    // Событие открытия балуна будем слушать только один раз.
    myPlacemark1.events.once('balloonopen', requestForPanorama);
    myPlacemark2.events.once('balloonopen', requestForPanorama);
    myPlacemark3.events.once('balloonopen', requestForPanorama);
    myPlacemark4.events.once('balloonopen', requestForPanorama);


    myMap.geoObjects.add(myPlacemark1);
    myMap.geoObjects.add(myPlacemark2);
    myMap.geoObjects.add(myPlacemark3);
    myMap.geoObjects.add(myPlacemark4);

        // Создаем на основе существующей модели мультимаршрут.
        multiRoute = new ymaps.multiRouter.MultiRoute(multiRouteModel, {
            // Путевые точки можно перетаскивать.
            // Маршрут при этом будет перестраиваться.
            wayPointDraggable: true,
            boundsAutoApply: true
        });

    // Добавляем мультимаршрут на карту.
    myMap.geoObjects.add(multiRoute);

    function changeRoutingMode(routingMode, targetItem) {
        multiRouteModel.setParams({ routingMode: routingMode }, true);

        // Отменяем выбор элементов.
        autoRouteItem.deselect();
        masstransitRouteItem.deselect();
        pedestrianRouteItem.deselect();

        // Выбираем элемент и закрываем список.
        targetItem.select();
        routeTypeSelector.collapse();
    }
}

ymaps.ready(init);
