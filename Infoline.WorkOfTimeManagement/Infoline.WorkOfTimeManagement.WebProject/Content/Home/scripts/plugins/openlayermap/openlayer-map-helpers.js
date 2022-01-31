InfolineMap.Constant = {
    Outside: 'Outside',
    Delete: 'Delete',
    Overlay: 'Overlay'
}

InfolineMap.DrawType = {
    Point: 'Point',
    Marker: 'Marker',
    LineString: 'LineString',
    Polygon: 'Polygon',
    LineStringArrow: 'LineStringArrow',
    DeletedPolygon: 'DeletedPolygon'
}

InfolineMap.DrawEvents = {
    Idle: 'Idle',
    All: 'All',
    Arrow: 'DrawArrow',
    Marker: 'DrawMarker',
    Draw: 'Draw'
}

InfolineMap.CommonStyle = {
    'LineStringArrow': function (feature) {
        var geometry = feature.getGeometry();

        var styles = [
            new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: '#ffcc33',
                    width: 2
                })
            })
        ];

        geometry.forEachSegment(function (start, end) {
            var dx = end[0] - start[0];
            var dy = end[1] - start[1];
            var rotation = Math.atan2(dy, dx);
            styles.push(new ol.style.Style({
                geometry: new ol.geom.Point(end),
                image: new ol.style.Icon({
                    src: viewModel.source + '/images/arrow.png',
                    anchor: [0.75, 0.5], //0.75,0.5
                    rotateWithView: true,
                    rotation: -rotation
                })
            }));
            /*  var a = end[0] - start[0];
              var b = end[1] - start[1];
              if (a && b)
                  var angle = Math.abs(b / a);
              if (a || b) {
                  var x = start[0];
                  var y = start[1];
                  var count_x = a < 0 ? -10 : a > 0 ? 10 : 0;
                  var count_y = (b < 0 ? -10 : b > 0 ? 10 : 0) * (angle ? angle : 1);
                  var flag = true;
                  while (flag) {
                      var old_x = x;
                      var old_y = y;
                      if (Math.abs(end[0] - x) > Math.abs(count_x))
                      { x = x + count_x; }
                      if (Math.abs(end[1] - y) > Math.abs(count_y))
                      { y = y + count_y; }
  
                      if (old_x == x && old_y == y) {
                          flag = false;
                      }
                      else {
                          styles.push(new ol.style.Style({
                              geometry: new ol.geom.Point([x, y]),
                              image: new ol.style.Icon({
                                  src: viewModel.source +  viewModel.source + '/images/arrow.png',
                                  anchor: [0.5, 0.5],
                                  rotateWithView: true,
                                  rotation: -rotation
                              })
                          }));
                      }
                  }
              }*/
        });
        result = styles;
        return result;
    },
    'Point': function () {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 7,
                fill: new ol.style.Fill({ color: 'blue' }),
                stroke: new ol.style.Stroke({ color: 'blue', width: 1 })
            })
        });
    },
    'OnlinePoint': function () {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 7,
                fill: new ol.style.Fill({ color: 'rgb(26, 179, 148)' }),
                stroke: new ol.style.Stroke({ color: 'rgb(26, 179, 148)', width: 1 })
            })
        });
    },
    'OfflinePoint': function () {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 7,
                fill: new ol.style.Fill({ color: 'rgb(237, 85, 101)' }),
                stroke: new ol.style.Stroke({ color: 'rgb(237, 85, 101)', width: 1 })
            })
        });
    },
    'Marker': function (feature) {
        var img, img_scale, anchor;
        if (feature) {
            img = feature.get('customStylePath');
            img_scale = feature.get('scale');
            anchor = feature.get('anchor');
        }


        return new ol.style.Style({
            image: new ol.style.Icon({
                src: (img && img != "") ? img : viewModel.source + '/images/red_marker.png',
                anchor: (anchor && anchor.length > 0) ? anchor : [0.5, 500],
                anchorXUnits: 'fraction',
                anchorYUnits: 'pixels',
                scale: img_scale || 0.08
            })
        });
    },
    'Photo': function (feature) {
        var iconSettings = feature.get('iconSettings');

        if (!feature) {
            console.log("Geçerli feature bulunamadı");
            return;
        }
        if (!iconSettings) {
            console.log("İkon ayarları bulunamadı");
            return;
        }

        return new ol.style.Style({
            image: new ol.style.Icon(iconSettings)
        });
    },
    'LineString': function () {
        return new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: 'green',
                width: 1
            })
        });
    },
    'Polygon': function () {
        return new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: 'blue',
                width: 3
            }),
            fill: new ol.style.Fill({
                color: 'rgba(0, 0, 255, 0.1)'
            })
        });
    },
    'DeletedPolygon': function () {
        return new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: 'blue',
                width: 3
            }),
            fill: new ol.style.Fill({
                color: 'rgba(255, 0, 0, 0.3)'
            })
        });
    },
    'Circle': function () {
        return new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: 'red',
                width: 2
            }),
            fill: new ol.style.Fill({
                color: 'rgba(255,0,0,0.2)'
            })
        });
    }
}
//map objesini oluştumak için çağrılan fonksiyondur.
function InfolineMap(obj) {
    var _this = this, _style = obj.style || {}, minZoom = obj.minZoom, center = obj.center = obj.center;

    _this.mapContainer = obj.target;
    _this.dataBoundFn = obj.dataBoundFn;
    _this.defaultExtent = null;

    _this.ClickState = InfolineMap.DrawEvents.Idle;
    _this._clicks = {};

    _this.NoneStyleFunction = null;

    if (!window.ol) {
        Object.keys(InfolineMap.prototype).forEach(function (key) {
            InfolineMap.prototype[key] = function () { }
        });

        _this.mapContainer.html($('[google-no-connection]').html().trim());
    } else {
        _this.mapId = _this.mapContainer.attr('id') + '_' + "".getGuid();

        _this.mapContainer.append('<div id="' + _this.mapId + '" class="mapDiv" style="height:' + (_this.mapContainer.css("height") || "500px") + '">');


        _this.raster = new ol.layer.Tile({
            source: new ol.source.OSM()
        });

        _this.vectorSource = new ol.source.Vector();

        _this.vectorLayer = new ol.layer.Vector({
            source: _this.vectorSource,
            style: function (feature) {
                function returnStyle(key) {
                    feature.set('customStyle', key);
                    if (_style[key]) {
                        return _style[key](feature);
                    } else {
                        return InfolineMap.CommonStyle[key](feature);
                    }

                }

                var key = feature.getGeometry().getType();
                if (key) {
                    var featureStyle = feature.get('customStyle');

                    if (!featureStyle) {
                        if (_this.NoneStyleFunction) {
                            featureStyle = _this.NoneStyleFunction(key, feature);

                            if (featureStyle) {
                                return returnStyle(featureStyle);
                            }
                        }
                    }

                    return returnStyle(featureStyle || key);
                } else {
                    return null;
                }
            }
        });
        _this.ol_map = new ol.Map({
            layers: [_this.raster, _this.vectorLayer],
            target: _this.mapId,
            view: new ol.View({
                center: ol.proj.transform(center && center.length ? center : [31.91520904, 77.2428528], 'EPSG:4326', 'EPSG:3857'),// [0, 0],
                zoom: 2,
                minZoom: minZoom || 1
            })
        });

        // haritaya click dinlemelerini yönetiyor.
        _this.ol_map.on('click', function (e) {
            var _arguments = arguments;
            if (_this._clicks[_this.ClickState]) {
                _this._clicks[_this.ClickState].forEach(function (fn) {
                    fn.apply(_this.ol_map, _arguments);
                });
            }
            if (_this.ClickState != 'all' && _this._clicks.all) {
                _this._clicks.all.forEach(function (fn) {
                    fn.apply(_this.ol_map, _arguments);
                });
            }
        });

        // feature hover için css
        _this.ol_map.on("pointermove", function (evt) {
            var hit = _this.ol_map.forEachFeatureAtPixel(evt.pixel, function (feature, layer) {
                return true;
            });
            if (hit) {
                $('#' + _this.ol_map.getTarget()).css("cursor", "pointer");
            } else {
                $('#' + _this.ol_map.getTarget()).css("cursor", "default");
            }
        });

        _this.ol_map.on('moveend', function (evt) {
            var map = evt.map;

            var extent = map.getView().calculateExtent(map.getSize());
            if (!_this.defaultExtent)
                _this.defaultExtent = extent;
            var bottomLeft = ol.extent.getBottomLeft(extent);
            var topRight = ol.extent.getTopRight(extent);
            var _bottom = bottomLeft[1];
            var flag = false;
            var _top = topRight[1];
            if (_bottom < -19598319.24923739) {//-19598319.24923739 , -7827151.696402052
                extent[1] = -19598319.24923739;
                flag = true;
            }
            if (_top > 19662069.930459257) {
                extent[3] = 19662069.930459257;
                flag = true;
            }
            if (flag) {
                try {
                    _this.ol_map.getView().fit( extent, _this.ol_map.getSize());
                } catch (err) {
                    _this.ol_map.getView().fit(_this.defaultExtent, _this.ol_map.getSize());
                }
            }
        });
    }
}
//haritada var olan featureları kapsayacak şekilde boundingi ayarlamayı sağlar. 
//İçerisinde zoom seviyeside ayarlanmıştır. Fazla zoom yapmaması için zoom seviyesi max 19 a ayarlanır.
InfolineMap.prototype.SetBounding = function (zoomLevel) {
    var _this = this;
    var coordinates = _this.GetCoordinates();
    if (coordinates.length != 0) {
        var ext = ol.extent.boundingExtent(coordinates);
        // _this.ol_map.getView().fit(ext, _this.ol_map.getSize());
        _this.ol_map.getView().fit(ext, _this.ol_map.getSize(), { maxZoom: 1 });
    } else {
        _this.ol_map.getView().setZoom(1);
    }
    if (_this.ol_map.getView().getZoom() > 19)
        _this.ol_map.getView().setZoom(19);
    if (zoomLevel)
        _this.ol_map.getView().setZoom(zoomLevel);
}
//haritanın bounding bilgisini almak için kullanılır.
InfolineMap.prototype.GetBounding = function () {
    var _this = this;
    var oldCoordinates = _this.ol_map.getView().calculateExtent(_this.ol_map.getSize());
    var temp = [];
    for (var i = 0; i < oldCoordinates.length; i = i + 2) {
        temp.push([oldCoordinates[i], oldCoordinates[i + 1]]);
    }

    temp = temp.map(function (xy) {
        xy = ol.proj.transform(xy, 'EPSG:3857', 'EPSG:4326');
        return xy;
    })
    return temp;
}
//haritanın height width ini tekrar ayarlar.
InfolineMap.prototype.Resize = function () {
    var _this = this;
    _this.ol_map.updateSize();
}
//haritada var olan featureların koordinatlarını döner
InfolineMap.prototype.GetCoordinates = function () {
    var _this = this;
    var coordinates = [];
    var features = _this.vectorLayer.getSource().getFeatures();
    features.forEach(function (feature) {
        var featureType = feature.getGeometry().getType();
        var array;
        if (featureType == InfolineMap.DrawType.Point) {
            coordinates.push(feature.getGeometry().getCoordinates());
        }
        else if (featureType == InfolineMap.DrawType.Polygon) {
            feature.getGeometry().getCoordinates()[0].forEach(function (point) {
                coordinates.push(point);
            });
        }
        else if (featureType == InfolineMap.DrawType.LineString) {
            feature.getGeometry().getCoordinates().forEach(function (point) {
                coordinates.push(point);
            });
        }
    });
    return coordinates;
}
//tıklanan pixeldeki feature'ı alır.
InfolineMap.prototype.GetFeaturesAtPixel = function (e) {
    return this.ol_map.getFeaturesAtPixel(e.pixel);
}
//feature'ı haritadan kalmık için kullanılır.
InfolineMap.prototype.RemoveFeature = function (feature) {
    this.vectorLayer.getSource().removeFeature(feature);
}
//harita üzerindeki featureların tipleri ile koordinatlarını döner (kendi korrdinat sistemine göre) {Points: [],LineStrings :[]} şeklinde
InfolineMap.prototype.GetData = function (obj) {
    obj = obj || {};
    var otherTypeFn = obj.otherTypeFn;

    var _this = this;
    var result = { Points: [], Markers: [], LineStrings: [], LineStringArrows: [], Polygons: [], HasData: false };

    var features = _this.vectorLayer.getSource().getFeatures();
    features.forEach(function (feature) {
        var selected = null;

        var featureType = feature.getGeometry().getType();

        if (featureType == InfolineMap.DrawType.Point) {
            if (feature.get('customStyle') == InfolineMap.DrawType.Marker)
                selected = result.Markers;
            else
                selected = result.Points;
        } else if (featureType == InfolineMap.DrawType.LineString) {
            if (feature.get('customStyle') == InfolineMap.DrawType.LineStringArrow)
                selected = result.LineStringArrows;
            else
                selected = result.LineStrings;
        } else if (featureType == InfolineMap.DrawType.Polygon) {
            selected = result.Polygons;
        } else if (otherTypeFn) {
            selected = otherTypeFn(result);
        }

        if (selected) {
            result.HasData = true;
            var obj = { coordinates: feature.getGeometry().getCoordinates() }
            if (featureType == InfolineMap.DrawType.Polygon)
                obj.inside = feature.get('inside');
            selected.push(obj);
        }
    });
    return result;
}
//harita üzerindeki featureların tipleri ile koordinatlarını döner (coğrafi koordinat sistemine göre) {Points: [],LineStrings :[]} şeklinde
InfolineMap.prototype.GetDataAsCoordinateSystem = function () {
    var _this = this;
    var data = _this.GetData();
    var points = data.Points, markers = data.Markers, linestrings = data.LineStrings, linestringarrows = data.LineStringArrows, polygons = data.Polygons;
    if (points.length)
        points = points.map(function (_point) {
            _point.coordinates = ol.proj.transform(_point.coordinates, 'EPSG:3857', 'EPSG:4326');
            return _point
        });
    if (markers.length)
        markers = markers.map(function (_marker) {
            _marker.coordinates = ol.proj.transform(_marker.coordinates, 'EPSG:3857', 'EPSG:4326');
            return _marker
        });

    if (linestrings.length)
        linestrings.forEach(function (one_linestring) {
            one_linestring = [one_linestring.coordinates.map(function (xy) {
                xy = ol.proj.transform(xy, 'EPSG:3857', 'EPSG:4326');
                return xy
            })];
        });
    if (linestringarrows.length)
        linestringarrows.forEach(function (one_linestring) {
            one_linestring.coordinates = one_linestring.coordinates.map(function (xy) {
                xy = ol.proj.transform(xy, 'EPSG:3857', 'EPSG:4326');
                return xy
            });
        });
    if (polygons.length)
        polygons.forEach(function (one_polygon) {
            one_polygon.coordinates[0] = one_polygon.coordinates[0].map(function (xy) {
                xy = ol.proj.transform(xy, 'EPSG:3857', 'EPSG:4326');
                return xy
            });
        });
    return { Points: points, Markers: markers, LineStrings: linestrings, LineStringArrows: linestringarrows, Polygons: polygons };
}
//propertysine "animate": true atanan featurelara yanıp sönme animasyonu vermek için kullanılır.
InfolineMap.prototype.Animate = function () {
    var _this = this;
    var features = _this.vectorLayer.getSource().getFeatures();
    features = features.filter(function (feature) {
        return feature.get('animate') == true;
    });
    var countR = 1;
    var countB = 0;
    setInterval(
        function (index) {

            var __style = new ol.style.Style({
                image: new ol.style.Circle({
                    radius: 7,
                    fill: new ol.style.Fill({ color: "rgba(237, 85, 101, " + countR + ")" }),
                    stroke: new ol.style.Stroke({ color: "rgb(237, 85, 101)", width: 3 })
                })
            });
            var temp = countR;
            countR = countB;
            countB = temp;
            features.forEach(function (one_feature) {
                one_feature.setStyle(__style);
            });
        },
        1000);
}
//propertysine "animateflash": true atanan featurelara flash (kırmızı dalga) animasyonu vermek için kullanılır.
InfolineMap.prototype.AnimateFlash = function (stop) {
    var _this = this;
    var features = _this.vectorLayer.getSource().getFeatures();
    features = features.filter(function (feature) {
        return feature.get('animateflash') == true;
    });

    features.forEach(function (marker) {
        var duration = 3000;
        function flash(feature, isStop) {
            var start = new Date().getTime();
            var listenerKey = null;
            if (stop == false) {
                listenerKey = _this.ol_map.un('postcompose', animate);
                listenerKey = null;
                $(_this.ol_map).off('postcompose');

                return false;
            }

            function animate(event) {
                if (stop == false)
                    listenerKey = null;
                var vectorContext = event.vectorContext;
                var frameState = event.frameState;
                var flashGeom = feature.getGeometry().clone();
                var elapsed = frameState.time - start;
                var elapsedRatio = elapsed / duration;
                // radius will be 5 at start and 30 at end.
                var radius = ol.easing.easeOut(elapsedRatio) * 20 + 5;
                var opacity = ol.easing.easeOut(1 - elapsedRatio);
                if (!stop)
                    opacity = 0;

                var style = new ol.style.Style({
                    image: new ol.style.Circle({
                        radius: radius,
                        snapToPixel: false,
                        stroke: new ol.style.Stroke({
                            color: 'rgba(255, 0, 0, ' + opacity + ')',
                            width: 1
                            //width: 0.25 + opacity
                        })
                    })
                });

                if (stop == true) {
                    vectorContext.setStyle(style);
                    vectorContext.drawGeometry(flashGeom);

                    if (elapsed > duration) {
                        ol.Observable.unByKey(listenerKey);

                        return;
                    }
                    _this.ol_map.render();
                }

            }

            listenerKey = _this.ol_map.on('postcompose', animate);
        }
        flash(marker);

        var interval = setInterval(function () { flash(marker); }, 1000);

        if (stop == false)
            clearInterval(interval);
    });
}

InfolineMap.prototype.AnimateMarkerMoveOnLine = function () {
    var _this = this;
    var linecor = _this.GetCoordinates();

    var routeFeature = new ol.Feature({
        type: 'Line',
        geometry: new ol.geom.LineString([linecor])
    });
    var geoMarker = new ol.Feature({
        type: 'geoMarker',
        geometry: new ol.geom.Point(linecor[0])
    });
    var startMarker = new ol.Feature({
        type: 'icon',
        geometry: new ol.geom.Point(linecor[0])
    });
    var endMarker = new ol.Feature({
        type: 'icon',
        geometry: new ol.geom.Point(linecor[linecor.length - 1])
    });



    var styles = {
        'Line': new ol.style.Style({
            stroke: new ol.style.Stroke({
                width: 6, color: [237, 212, 0, 0.8]
            })
        }),
        'icon': new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [0.5, 1],
                src: viewModel.source + '/images/icon.png'
            })
        }),
        'geoMarker': new ol.style.Style({
            image: new ol.style.Circle({
                radius: 7,
                snapToPixel: false,
                fill: new ol.style.Fill({ color: 'black' }),
                stroke: new ol.style.Stroke({
                    color: 'white', width: 2
                })
            })
        })
    };

    var animating = false;
    var speed, now;

    var vectorLayer = new ol.layer.Vector({
        source: new ol.source.Vector({
            features: [routeFeature, geoMarker, startMarker, endMarker]
        }),
        style: function (feature) {
            // hide geoMarker if animation is active
            if (animating && feature.get('type') === 'geoMarker') {
                return null;
            }
            return styles[feature.get('type')];
        }
    });
    _this.vectorLayer = vectorLayer;
    _this.ol_map.loadTilesWhileAnimating = true;

    var moveFeature = function (event) {
        var vectorContext = event.vectorContext;
        var frameState = event.frameState;

        if (animating) {
            var elapsedTime = frameState.time - now;
            // here the trick to increase speed is to jump some indexes
            // on lineString coordinates
            var index = Math.round(speed * elapsedTime / 1000);

            if (index >= linecor.length) {
                stopAnimation(true);
                return;
            }

            var currentPoint = new ol.geom.Point(linecor[index]);
            var feature = new ol.Feature(currentPoint);
            vectorContext.drawFeature(feature, styles.geoMarker);
        }
        // tell OpenLayers to continue the postcompose animation
        _this.ol_map.render();
    };
    startAnimation();
    function startAnimation() {
        if (animating) {
            stopAnimation(false);
        } else {
            animating = true;
            now = new Date().getTime();
            speed = 10;
            // hide geoMarker
            geoMarker.setStyle(null);
            // just in case you pan somewhere else
            _this.SetBounding();
            _this.ol_map.on('postcompose', moveFeature);
            _this.ol_map.render();
        }
    }


    function stopAnimation(ended) {
        animating = false;

        // if animation cancelled set the marker at the beginning
        var coord = ended ? linecor[linecor.length - 1] : linecor[0];
        /** @type {ol.geom.Point} */ (geoMarker.getGeometry())
            .setCoordinates(coord);
        //remove listener
        _this.ol_map.un('postcompose', moveFeature);
    }

}
//propertysine "animateflash": true atanan featurelara flash (kırmızı dalga) animasyonu vermek için kullanılır.
InfolineMap.prototype.Clear = function () {
    this.vectorSource.clear(true);
}

InfolineMap.prototype.Draw = function (value) {
    var _this = this;
    var draw = new ol.interaction.Draw({
        source: _this.vectorSource,
        type: InfolineMap.DrawType[value]
    });

    draw.on('drawend', function (e) {
        var currentFeature = e.feature;
        var restOfFeats = _this.vectorSource.getFeatures();
        var allFeats = restOfFeats.concat(currentFeature);
        if (_this.dataBoundFn)
            _this.dataBoundFn({ HasData: allFeats.length > 0 });
    });

    return draw;
}

InfolineMap.prototype.AnimateFlashHp = function (stop) {
    var _this = this;
    if (_this.intervals == null) { _this.intervals = {} }

    var features = _this.vectorLayer.getSource().getFeatures();
    features = features.filter(function (feature) {
        return feature.get('animateflash') == true;
    });
    var duration = 1000;
    features.forEach(function (feat) {
        var listenerKey = null;
        function flash(feature, isStop) {
            var start = new Date().getTime();

            var vectorContext, frameState, flashGeom, elapsed, elapsedRatio, radius, opacity, style, rendering = false;

            function animate(event) {
                vectorContext = event.vectorContext;
                frameState = event.frameState;
                flashGeom = feature.getGeometry().clone();
                elapsed = new Date().getTime() - start;
                elapsedRatio = elapsed / duration;
                radius = ol.easing.easeOut(elapsedRatio) * 20 + 5;
                opacity = ol.easing.easeOut(1 - elapsedRatio);

                style = new ol.style.Style({
                    image: new ol.style.Circle({
                        radius: radius,
                        snapToPixel: false,
                        stroke: new ol.style.Stroke({
                            color: 'rgba(255, 0, 0, ' + opacity + ')',
                            width: 1
                        })
                    })
                });

                vectorContext.setStyle(style);
                vectorContext.drawGeometry(flashGeom);

                if (elapsed > duration) {
                    ol.Observable.unByKey(listenerKey);
                    listenerKey = _this.ol_map.un('postcompose');
                    delete listenerKey;
                    $(_this.ol_map).off('postcompose');

                    opacity = 0;
                    return;
                } else {
                    _this.ol_map.render();
                }


            }

            if (listenerKey == null) {
                listenerKey = _this.ol_map.on('postcompose', function (evt) {
                    animate(evt)
                });

                _this.ol_map.render();
            }
        }

        _this.intervals[feat.get('UniqueId')] = setInterval(function () {
            flash(feat);
        }, duration)
    })
}

InfolineMap.prototype.SetData = function (geoObject, willTranslate) {
    var _this = this;

    if (!geoObject)
        return false;
    else
        geoObject.features.forEach(function (feature) {
            function translateCoor(coors) {
                coors.forEach(function (one_latlon, index) {//gelen objede one_latlon[0] db'deki longitude one_latlon[1] db'deki latitude 
                    coors[index] = ol.proj.transform(one_latlon, 'EPSG:4326', 'EPSG:3857');
                });
                return coors;
            }
            if (feature.geometry.type == InfolineMap.DrawType.Polygon) {
                if (willTranslate)
                    feature.geometry.coordinates = [translateCoor(feature.geometry.coordinates[0])];
                else
                    feature.geometry.coordinates = feature.geometry.coordinates;
            }
            else if (feature.geometry.type == InfolineMap.DrawType.Point) {
                if (willTranslate)
                    feature.geometry.coordinates = translateCoor([feature.geometry.coordinates])[0];
                else
                    feature.geometry.coordinates = feature.geometry.coordinates;


            }
            else if (feature.geometry.type != InfolineMap.DrawType.Polygon) {//linestring ise
                if (willTranslate)
                    feature.geometry.coordinates = translateCoor(feature.geometry.coordinates);
                else
                    feature.geometry.coordinates = feature.geometry.coordinates;
            }
        });

    _this.vectorSource.clear(true);
    _this.vectorSource.addFeatures((new ol.format.GeoJSON()).readFeatures(geoObject));
    if (_this.dataBoundFn)
        _this.dataBoundFn(_this.GetData());

    if (_this.intervals) {
        Object.keys(_this.intervals).forEach(function (key) {
            clearInterval(_this.intervals[key]);
        });
        delete _this.intervals;
    }
}

InfolineMap.prototype.EachFeatures = function (func) {
    var _this = this;
    if (!func)
        return false;
}

InfolineMap.prototype.RemoveInteraction = function (item) {
    this.ol_map.removeInteraction(item);
}

InfolineMap.prototype.AddInteraction = function (item) {
    this.ol_map.addInteraction(item);
}

InfolineMap.prototype.Click = function (state, fn) {
    if (!this._clicks[state])
        this._clicks[state] = [fn];
    else
        this._clicks[state].push(fn);
}

InfolineMapActions.Language = {
    "point": "Nokta",
    "marker": "İşaretleyici",
    "polygon": "Poligon",
    "linestring": "Çizgi",
    "linestringarrow": "Ok",
    "delete": "Sil",
    "overlay": "Açıklama",
    "outside": "Dışını Seç",
    "none": "Seçimi Bırak"
};

InfolineMapActions.Language = (typeof viewmodel != "undefined" && viewModel.infolineMapActionLanguage);

InfolineMap.prototype.SearchAddress = function () {
    //detay ve bilgiler için: https://www.npmjs.com/package/ol3-geocoder
    var _this = this;
    var map = _this.ol_map;
    var geocoder = new Geocoder('nominatim', {
        provider: 'photon',
        lang: 'tr',
        placeholder: 'Adres Ara ...',
        limit: 5,
        debug: false,
        autoComplete: true,
        keepOpen: true
    });
    if (!geocoder)
        return false;
    map.addControl(geocoder);

    //Listen when an address is chosen
    geocoder.on('addresschosen', function (evt) {
        console.info(evt);
        evt.feature.setProperties({ 'SearchAddres': true });
    });

}

function InfolineMapActions(obj) {
    var _this = this;
    var infoline_map = obj.infoline_map;

    var events = {
        delete: 'delete',
        outsideInside: 'outside&inside'
    }

    this.Point = obj.actions.Point;
    this.Marker = obj.actions.Marker;
    this.LineString = obj.actions.LineString;
    this.LineStringArrow = obj.actions.LineStringArrow;
    this.Polygon = obj.actions.Polygon;
    this.Delete = obj.actions.Delete;
    this.Outside = obj.actions.Outside;
    this.None = true;

    infoline_map.mapContainer.prepend('<div class="tool-container toolButtons">' +
        '<div class="ol-control default-control clearfix" data-actions="true"></div></div>');

    if (this.Point)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="Point" title="' + InfolineMapActions.Language.point + '" data-placement="right"><i class="fa fa-stop-circle"></i></button>');
    if (this.Marker)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="Marker" data-placement="right" title="' + InfolineMapActions.Language.marker + '"><i class="fa fa-map-marker"></i></button>');
    if (this.LineString)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="LineString" data-placement="right" title="' + InfolineMapActions.Language.linestring + '"><i class="fa fa-pencil"></i></button>');
    if (this.LineStringArrow)
        infoline_map.mapContainer.find('[data-actions="true"]').append(' <button type="button" data-tool="LineStringArrow"  data-placement="right" title="' + InfolineMapActions.Language.linestringarrow + '"><i class="fa fa-exchange"></i></button>');
    if (this.Polygon)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="Polygon"  data-placement="right" title="' + InfolineMapActions.Language.polygon + '"><i class="fa fa-th-large"></i></button>');
    if (this.Delete)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="Delete"  data-placement="right" title="' + InfolineMapActions.Language.delete + '"><i class="fa fa-eraser"></i></button>');
    if (this.Outside)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="Outside"  data-placement="right" title="' + InfolineMapActions.Language.outside + '"><i class="fa fa-bandcamp"></i></button>');
    if (this.None)
        infoline_map.mapContainer.find('[data-actions="true"]').append('<button type="button" data-tool="None"  data-placement="right" title="' + InfolineMapActions.Language.none + '"><i class="fa fa-ban"></i></button>');

    var oldDraw;
    infoline_map.mapContainer.find('[data-tool]').click(function (e) {
        var value = $(this).attr("data-tool");

        if (oldDraw)
            infoline_map.RemoveInteraction(oldDraw);

        infoline_map.NoneStyleFunction = null;

        if (value == InfolineMap.Constant.Delete) {
            infoline_map.ClickState = events.delete;
        } else if (value == InfolineMap.Constant.Outside) {
            infoline_map.ClickState = events.outsideInside;
        } else if (value != 'None') {

            if (value == InfolineMap.DrawType.LineStringArrow) {
                infoline_map.ClickState = InfolineMap.DrawEvents.Arrow;
                value = InfolineMap.DrawType.LineString;
                infoline_map.NoneStyleFunction = function (key, feature) {
                    if (InfolineMap.DrawType.LineString == key) {
                        feature.set('customStyle', InfolineMap.DrawType.LineStringArrow);
                        return InfolineMap.DrawType.LineStringArrow;
                    }
                    return key;
                }
            } else if (value == InfolineMap.DrawType.Marker) {
                infoline_map.ClickState = InfolineMap.DrawEvents.Marker;
                value = InfolineMap.DrawType.Point;
                infoline_map.NoneStyleFunction = function (key, feature) {
                    if (InfolineMap.DrawType.Point == key) {
                        feature.set('customStyle', InfolineMap.DrawType.Marker);
                        return InfolineMap.DrawType.Marker;
                    }
                    return key;
                }
            } else {
                infoline_map.ClickState = InfolineMap.DrawEvents.Draw;
            }
            oldDraw = infoline_map.Draw(value);

            infoline_map.AddInteraction(oldDraw);

        } else {
            infoline_map.ClickState = InfolineMap.DrawEvents.Idle;
        }
    });

    infoline_map.Click(events.delete, function (e) {
        var features = infoline_map.GetFeaturesAtPixel(e);
        if (features && features.length) {
            infoline_map.RemoveFeature(features[0])

            if (infoline_map.dataBoundFn)
                infoline_map.dataBoundFn(infoline_map.GetData());
        }
    });

    infoline_map.Click(events.outsideInside, function (e) {
        var features = infoline_map.GetFeaturesAtPixel(e);
        if (features && features.length) {
            var feature = features[0];

            if (feature.getGeometry().getType() == InfolineMap.DrawType.Polygon) {
                if (!(feature.get('inside') == false))
                    style = InfolineMap.CommonStyle[InfolineMap.DrawType.DeletedPolygon]();
                else
                    style = InfolineMap.CommonStyle[InfolineMap.DrawType.Polygon]();
                feature.set('inside', feature.get('inside') == false ? true : false);
                feature.setStyle(style);
            }
        }
    })
}

InfolineMapActions.prototype.AddButton = function (obj) {
    var infoline_map = obj.infoline_map;
    var type = obj.type;
    var icon = obj.icon;
    var title = obj.title;
    var clickFuncs = obj.clickFuncs;

    if (!infoline_map || !type || !icon || !infoline_map.mapContainer || !infoline_map.mapContainer.length || !clickFuncs || !clickFuncs.length)
        return false
    var buttonContainer = infoline_map.mapContainer.find('[data-actions="true"]');
    if (buttonContainer.length == 0 || infoline_map.mapContainer.find("[data-tool='" + type + "']").length)
        return false
    buttonContainer.append('<button type="button" data-tool="' + type + '" data-placement="right" title=' + title + '><i class="' + icon + '"></i></button>');
    clickFuncs.forEach(function (oneclick) {
        if (typeof oneclick == "function") {
            if (infoline_map.mapContainer.find("[data-tool='" + type + "']").length) {
                infoline_map.mapContainer.find("[data-tool='" + type + "']").click(oneclick)
            }
        }
    });
}

InfolineMapActions.prototype.SetButtonVisibility = function (obj) {
    var infoline_map = obj.infoline_map;
    if (!infoline_map)
        return false;
    var _point = obj.Point, _marker = obj.Marker, _linestring = obj.LineString, _linestringarrow = obj.LineStringArrow, _polygon = obj.Polygon, _delete = obj.Delete, _outside = obj.Outside, _none = obj.None;

    if (this.Point) {
        if (_point == true)
            infoline_map.mapContainer.find("[data-tool='Point']").show()
        else if (_point == false)
            infoline_map.mapContainer.find("[data-tool='Point']").hide()
    }
    if (this.Marker) {
        if (_marker == true)
            infoline_map.mapContainer.find("[data-tool='Marker']").show()
        else if (_marker == false)
            infoline_map.mapContainer.find("[data-tool='Marker']").hide()
    }
    if (this.LineString) {
        if (_linestring == true)
            infoline_map.mapContainer.find("[data-tool='LineString']").show()
        else if (_linestring == false)
            infoline_map.mapContainer.find("[data-tool='LineString']").hide()
    }
    if (this.LineStringArrow) {
        if (_linestringarrow == true)
            infoline_map.mapContainer.find("[data-tool='LineStringArrow']").show()
        else if (_linestringarrow == false)
            infoline_map.mapContainer.find("[data-tool='LineStringArrow']").hide()
    }
    if (this.Polygon) {
        if (_polygon == true)
            infoline_map.mapContainer.find("[data-tool='Polygon']").show()
        else if (_polygon == false)
            infoline_map.mapContainer.find("[data-tool='Polygon']").hide()
    }
    if (this.Delete) {
        if (_delete == true)
            infoline_map.mapContainer.find("[data-tool='Delete']").show()
        else if (_delete == false)
            infoline_map.mapContainer.find("[data-tool='Delete']").hide()
    }
    if (this.Outside) {
        if (_outside == true)
            infoline_map.mapContainer.find("[data-tool='Outside']").show()
        else if (_outside == false)
            infoline_map.mapContainer.find("[data-tool='Outside']").hide()
    }
    if (this.None) {
        if (_none == true)
            infoline_map.mapContainer.find("[data-tool='None']").show()
        else if (_none == false)
            infoline_map.mapContainer.find("[data-tool='None']").hide()
    }



};

function InfolineMapOverlay(infoline_map, e, overlayHtml) {
    var mapName = e.map.getTarget();
    var Overlay = new ol.Overlay({
        autoPan: true,
        autoPanAnimation: {
            duration: 250
        }
    });


    if ($("#popup_" + mapName).length == 0) {
        $("#" + mapName).append('<div id="popup_' + mapName + '" class="ol-popup" style="display:none"><a href= "#" id="popup-closer_' + mapName + '" class="ol-popup-closer"></a>  <div id="popup-content_' + mapName + '"></div></div>');
    }
    var popup = $("#popup_" + mapName);
    popup.css("max-height", $('#' + mapName).height() - 10);
    popup.css("right", "0px !Important");
    popup.css("position", "absolute !Important");
    if (popup.css('display') == 'none')
        popup.slideDown();

    document.getElementById("popup-closer_" + mapName).onclick = function () {
        popup.slideUp();
        Overlay.setPosition(undefined);
        return false;
    };

    Overlay.element = document.getElementById('popup_' + mapName);

    var coordinate = e.coordinate;
    Overlay.setPosition(coordinate);

    if (!e.map.overlays)
        e.map.overlays = [Overlay];
    else
        e.map.overlays.push(Overlay);


    document.getElementById("popup-content_" + mapName).innerHTML = overlayHtml;
}


function InfolineMapOverlaySide(infoline_map, e, overlayContent, align, ModalTitle) {
    var mapName = e.map.getTarget();
    var Overlay = new ol.Overlay({
        autoPan: true,
        autoPanAnimation: {
            duration: 250
        }
    });

    ModalTitle = ModalTitle ? ModalTitle : "" /*"DETAY BİLGİSİ"*/;
    align = align == "right" ? "ol-popupRight" : align == "left" ? "ol-popupLeft" : "ol-popupRight";

    if ($("#popup_" + mapName).length == 0) {
        $('#' + mapName).append('<div id="popup_' + mapName + '" class="' + align + '" style="display:none"><a href= "#" id="popup-closer_' + mapName + '" class="ol-popup-closer" style="z-index:9999;color:white"></a>  <div id="popup-content_' + mapName + '"></div></div>');
    }
    var popup = $("#popup_" + mapName);
    popup.css("max-height", $('#' + mapName).height());

    if (popup.css('display') == 'none')
        popup.slideDown();

    document.getElementById("popup-closer_" + mapName).onclick = function () {
        popup.slideUp();
        Overlay.setPosition(undefined);
        return false;
    };

    Overlay.element = document.getElementById('popup_' + mapName);

    var coordinate = e.coordinate;
    Overlay.setPosition(coordinate);

    if (!e.map.overlays)
        e.map.overlays = [Overlay];
    else
        e.map.overlays.push(Overlay);
    var notification = "notifications_" + mapName;
    var notifiContent = "notContent_" + mapName;
    var overlayHtml = '<div id="' + notification + '" class="myModal" style="top: 50%;min-height:' + ($('#' + mapName).height() - 2) + 'px;background: rgba(2,46,80,0.8);color: #fefefe !important;padding: 20px 0px;">' +
        '<div class="body clearfix">' +
        '<div class="column" style="width:100%;padding: 0 20px;">' +
        '<div><strong>' + ModalTitle + '</strong></div>' +
        '<hr>' +
        '<div id="' + notifiContent + '" ></div>'

    '</div>' +
        ' </div>' +
        '</div>';

    // NOTIFICONTENT İÇİN ÖRNEK KULLANIM ALTTADIR. Kullanılan Sayfadan gönderiniz.

    //var ornek = '<div class="form-group">' +
    //    '<div><strong>Bilgilendirme Detayı: </strong></div>' +
    //    '<div class="clearfix" data-column="Details">jhvjh</div>' +
    //    '</div>';


    //overlayHtml = '<div id="notifications" class="myModal" style="top: 50%;">' +
    //    '<div class="header clearfix">' +
    //    '<strong>SİNEK BİLDİRİMİ</strong>' +
    //    '<strong data-task="close" class="pull-right">X</strong>' +
    //    '</div>' +
    //    '<div class="body clearfix">' +
    //    '<div class="column" style="width:35%">' +
    //    '<div><strong>BİLDİRİM BİLGİLERİ</strong></div>' +
    //    '<hr>' +
    //    '<div class="form-group">' +
    //    '<div><strong>Bilgilendirme Yapan Kişi: </strong></div>' +
    //    '<div class="clearfix" data-column="createdby_Title">ahmet disli</div>' +
    //    '</div>' +
    //    '<div class="form-group">' +
    //    ' <div><strong>Bilgilendirme Tarihi: </strong></div>' +
    //    '<div class="clearfix" data-column="InformativeDate" data-type="date">06.03.2018</div>' +
    //    '</div>' +
    //    '<div class="form-group">' +
    //    '<div><strong>Bilgilendirme Tipi: </strong></div>' +
    //    ' <div class="clearfix" data-column="InformationType_Title">Sinek Çeşitliliği Bildirimi</div>' +
    //    '</div>' +
    //    ' <div class="form-group">' +
    //    '<div><strong>Bilgilendirme Detayı: </strong></div>' +
    //    ' <div class="clearfix" data-column="Details">jhvjh</div>' +
    //    ' </div>' +
    //    '  <div class="form-group">' +
    //    '   <button type="button" onclick="page.helper.modal.open()" class="btn btn-info btn-block">Ekip Yönlendir</button>' +
    //    ' </div>' +
    //    '</div>' +
    //    '<div class="column" style="width:65%">' +
    //    ' <div><strong>BİLDİRİM FOTOĞRAFLARI</strong></div>' +
    //    ' <hr>' +
    //    '<div id="photos" class="photos"><a href="http://78.188.170.4:3477/Upload/ENV_EnvironmentalInformationPhoto/5101ca1c-da5e-4cb3-9bd9-c12a50dd1a00/2018_03_06_17_45_04_b5e139f17ea34eb08cb45e442597a9f1.png" class="galery cboxElement"><img src="http://78.188.170.4:3477/Upload/ENV_EnvironmentalInformationPhoto/5101ca1c-da5e-4cb3-9bd9-c12a50dd1a00/2018_03_06_17_45_04_b5e139f17ea34eb08cb45e442597a9f1.png" width="100" height="100"></a></div>' +
    //    '</div>' +
    //    ' </div>' +
    //    '</div>';


    document.getElementById("popup-content_" + mapName).innerHTML = overlayHtml;
    $("#" + notifiContent).append(overlayContent);
}

function InfolineMapOverlayCenter(infoline_map, e, overlayContent, ModalTitle, func) {
    var mapName = e.map.getTarget();
    var Overlay = new ol.Overlay({
        autoPan: true,
        autoPanAnimation: {
            duration: 250
        }
    });



    ModalTitle = ModalTitle ? ModalTitle : ""; /*"DETAY BİLGİSİ";*/
    if ($("#popup_" + mapName).length == 0) {
        $('#' + mapName).append('<div id="popup_' + mapName + '" class="ol-popupCenter" style="display:none"><a href= "#" id="popup-closer_' + mapName + '" class="ol-popup-closer" style="z-index:99;color:white"></a>  <div id="popup-content_' + mapName + '"></div></div>');
    }
    var popup = $("#popup_" + mapName);
    popup.css("max-height", $('#' + mapName).height() / 2);

    if (popup.css('display') == 'none')
        popup.slideDown();

    document.getElementById("popup-closer_" + mapName).onclick = function () {
        popup.slideUp();
        Overlay.setPosition(undefined);
        return false;
    };

    Overlay.element = document.getElementById('popup_' + mapName);

    var coordinate = e.coordinate;
    Overlay.setPosition(coordinate);

    if (!e.map.overlays)
        e.map.overlays = [Overlay];
    else
        e.map.overlays.push(Overlay);
    var notification = "notifications_" + mapName;
    var notifiContent = "notContent_" + mapName;
    var overlayHtml = '<div id="' + notification + '" class="myModal" style="top: 50%;min-height:' + (($('#' + mapName).height() / 2) - 2) + 'px;background: rgba(2,46,80,0.8);color: #fefefe !important;padding: 20px 0px;">' +
        '<div class="body clearfix">' +
        '<div class="column" style="width:100%;padding: 0 20px;">' +
        '<div><strong>' + ModalTitle + '</strong></div>' +
        '<hr>' +
        '<div id="' + notifiContent + '" ></div>'

    '</div>' +
        ' </div>' +
        '</div>';

    // NOTIFICONTENT İÇİN ÖRNEK KULLANIM ALTTADIR. Kullanılan Sayfadan gönderiniz.

    //var ornek = '<div class="form-group">' +
    //    '<div><strong>Bilgilendirme Detayı: </strong></div>' +
    //    '<div class="clearfix" data-column="Details">jhvjh</div>' +
    //    '</div>';




    document.getElementById("popup-content_" + mapName).innerHTML = overlayHtml;
    $("#" + notifiContent).append(overlayContent);
}




