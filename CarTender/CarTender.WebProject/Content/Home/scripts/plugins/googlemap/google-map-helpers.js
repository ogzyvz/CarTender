if (window.google == null)
    window.google = {
        maps: {
            MapTypeControlStyle: {},
            Animation: {},
            ControlPosition: {
                TOP_CENTER: 'TOP_CENTER'
            },
            ZoomControlStyle: {},
            MapTypeId: {},
            SymbolPath: {},
            Marker: function () {
                return {
                    setAnimation: function () { return {}; },
                    infowindow: {},
                    addListener: function () { return {}; },
                    position: {
                        lat: function () { return {}; },
                        lng: function () { return {}; },
                    }
                }
            },
            LatLngBounds: function () {
                return {
                    extend: function () { return {}; },
                };
            },
            Polygon: function () { return {}; },
            Polyline: function () { return {}; },
            event: {
                addDomListener: function () { return {}; },
                trigger: function () { return {}; },
                addListener: function () { return {}; },
            },
            drawing: {
                DrawingManager: function () {
                    return {
                        setMap: {
                            call: function () { return {}; },
                        }
                    };
                },
            },
            Map: function (container) {
                $(container).html($('[google-no-connection]').html().trim());

                return {
                    fitBounds: function () { return {}; },
                    setZoom: function () { return {}; },
                    setMapTypeId: function () { return {}; },
                    mapTypes: {
                        set: function () { return {}; },
                    },
                    controls: {
                        "TOP_CENTER": []
                    }
                };
            },
            LatLng: function () { return {}; },
            InfoWindow: function () {
                return {
                    close: function () { return {}; },
                    open: function () { return {}; },
                };
            },
            Geocoder: {
                geocode: function () { return {}; },
            },
            Size: function () { return {}; },
            Point: function () { return {}; },
            ImageMapType: function () { return {}; },
        }
    };

google.maps.MapStyles = {
    BlueWater: [{ "featureType": "administrative", "elementType": "labels.text.fill", "stylers": [{ "color": "#444444" }] }, { "featureType": "administrative", "elementType": "labels.text.stroke", "stylers": [{ "color": "#f2f2f2" }] }, { "featureType": "landscape", "elementType": "all", "stylers": [{ "color": "#f2f2f2" }] }, { "featureType": "poi", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "road", "elementType": "all", "stylers": [{ "saturation": -100 }, { "lightness": 45 }] }, { "featureType": "road.highway", "elementType": "all", "stylers": [{ "visibility": "simplified" }] }, { "featureType": "road.arterial", "elementType": "labels.icon", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "elementType": "all", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "elementType": "all", "stylers": [{ "color": "#46bcec" }, { "visibility": "on" }] }],
    Water: [{ "featureType": "water", "stylers": [{ "saturation": 43 }, { "lightness": -11 }, { "hue": "#0088ff" }] }, { "featureType": "road", "elementType": "geometry.fill", "stylers": [{ "hue": "#ff0000" }, { "saturation": -100 }, { "lightness": 99 }] }, { "featureType": "road", "elementType": "geometry.stroke", "stylers": [{ "color": "#808080" }, { "lightness": 54 }] }, { "featureType": "landscape.man_made", "elementType": "geometry.fill", "stylers": [{ "color": "#ece2d9" }] }, { "featureType": "poi.park", "elementType": "geometry.fill", "stylers": [{ "color": "#ccdca1" }] }, { "featureType": "road", "elementType": "labels.text.fill", "stylers": [{ "color": "#767676" }] }, { "featureType": "road", "elementType": "labels.text.stroke", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "poi", "stylers": [{ "visibility": "off" }] }, { "featureType": "landscape.natural", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#b8cb93" }] }, { "featureType": "poi.park", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.sports_complex", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.medical", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.business", "stylers": [{ "visibility": "simplified" }] }]
}

google.maps.MapTypes = {
    Tarbil: {
        Name: 'Tarbil',
        Tiler: function (ms) {
            var _tiler = new google.maps.ImageMapType({
                ms: ms,
                ticks: (new Date()).getByFormatDeep().getTicks(),
                getTileUrl: function (coord, zoom) {
                    var _tlr = this;
                    var url = "http://tarbil.com/data/mapsvc?l=tarbil";
                    return url + "&z=" + zoom + "&x=" + coord.x + "&y=" + coord.y + "&s=" + _tlr.ms + "&t=" + _tlr.ticks + "&lev=ozc.jpg";
                },
                tileSize: new google.maps.Size(256, 256),
                isPng: false,
                name: "Tarbil",
                maxZoom: 19,
                minZoom: 0,
            });
            return _tiler;
        },
        Set: function (map, ms) {
            var _maptiler = google.maps.MapTypes.Tarbil.Tiler(ms);
            map.mapTypes.set('Tarbil', _maptiler);
        }
    },
    Bing: {
        Name: 'Bing',
        Tiler: function () {
            function TileXYToQuadKey(tileX, tileY, levelOfDetail) {
                quadKey = "";
                for (var i = levelOfDetail; i > 0; i--) {
                    var digit = '0';
                    var mask = 1 << (i - 1);
                    if ((tileX & mask) != 0) {
                        digit++;
                    }
                    if ((tileY & mask) != 0) {
                        digit++;
                        digit++;
                    }
                    quadKey += (digit);
                }
                return quadKey;
            }

            return new google.maps.ImageMapType({
                getTileUrl: function (coord, zoom) {
                    var url = "http://t0.tiles.virtualearth.net/tiles/a";
                    return url + TileXYToQuadKey(coord.x, coord.y, zoom) + ".jpeg?g=854&mkt=tr-TR&token=Anz84uRE1RULeLwuJ0qKu5amcu5rugRXy1vKc27wUaKVyIv1SVZrUjqaOfXJJoI0";
                },
                tileSize: new google.maps.Size(256, 256),
                maxZoom: 19,
                minZoom: 0,
                isPng: false,
                name: "Bing"
            });
        },
        Set: function (map) {
            var _bingTiler = google.maps.MapTypes.Bing.Tiler();
            map.mapTypes.set('Bing', _bingTiler);
        }
    },
    Spot6: {
        Name: 'Spot6',
        Tiler: function () {
            return new google.maps.ImageMapType({
                getTileUrl: function (coord, zoom) {
                    var url = "http://geowebcache.tarim.gov.tr/tile/service/tms/1.0.0/Spot6/";
                    return url + zoom + "/" + coord.x + "/" + (Math.pow(2, zoom) - coord.y - 1) + ".jpeg";
                },
                tileSize: new google.maps.Size(256, 256),
                isPng: false,
                name: "Spot6",
                maxZoom: 21,
                minZoom: 0
            });
        },
        Set: function (map) {
            var _maptiler = google.maps.MapTypes.Spot6.Tiler();
            map.mapTypes.set('Spot6', _maptiler);
        }
    }
}