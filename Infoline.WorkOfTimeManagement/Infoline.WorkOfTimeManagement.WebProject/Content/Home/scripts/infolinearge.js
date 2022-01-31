Number.prototype.toPointedString = function (fixed) {
    if (fixed && fixed.lang)
        lang = fixed.lang;
    else
        lang = 'tr-TR';

    if (fixed) {
        return this.toLocaleString(lang, {
            maximumFractionDigits: fixed
        })

    } else
        return this.toLocaleString(lang);
}

Number.prototype.getDate = function () {
    var nmbr = this < 621355968000010000 ? 621355968000010000 : this;
    return new Date((nmbr - 621355968000000000) / 10000);
}

Number.prototype.addTimezoneDifference = function () {
    return this + new Date().getTimezoneDifference();
}

Number.prototype.getStringNumber = function (obj) {
    obj = obj || {};
    var prefix = obj.prefix,
        suffix = obj.suffix,
        character = obj.character || '0';
    var stringNumber = this.toString();
    var limit = (obj.length || stringNumber.length) - stringNumber.length;

    if (suffix) {
        for (var count = 0; count < limit; count++)
            stringNumber = stringNumber.concat(character);
    } else {
        for (var count = 0; count < limit; count++)
            stringNumber = character.concat(stringNumber);
    }

    return stringNumber;
}


Number.prototype.formatToTime = function (obj) {
    var obj = obj || {};
    var locale = obj.locale, _this = this;
    var _columns = {
        "year": "year",
        "month": "month",
        "week": "week",
        "day": "day",
        "hour": "hour",
        "minute": "minute",
        "second": "second"
    }

    var date = new Date(_this);
    var dateStack = {};


    dateStack[_columns.year] = date.getUTCFullYear() - 1970;
    dateStack[_columns.month] = date.getUTCMonth();
    dateStack[_columns.day] = date.getUTCDate() - 1;
    dateStack[_columns.hour] = date.getUTCHours();
    dateStack[_columns.minute] = date.getUTCMinutes();
    dateStack[_columns.second] = date.getUTCSeconds();

    Object.keys(dateStack).forEach(function (key) {
        if (dateStack[key] <= 0) delete dateStack[key];
    });

    if (locale == null) {
        return dateStack;
    }
    else {
        var returnString = "";
        Object.keys(dateStack).forEach(function (key) {
            returnString += (dateStack[key] + " " + locale[key] + " ");
        });

        return returnString;
    }
}

Date.juFormat = {
    d4: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
    d3: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
    d2: ["00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"],
    d1: ["00", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"],
    M4: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
    M3: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
    M2: ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"],
    M1: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"]
}

Date.prototype.format = function (format) {
    var _this = this;
    var d = [];
    d[4] = Date.juFormat.d4;
    d[3] = Date.juFormat.d3;
    d[2] = Date.juFormat.d2;
    d[1] = Date.juFormat.d1;
    var M = [];
    M[4] = Date.juFormat.M4;
    M[3] = Date.juFormat.M3;
    M[2] = Date.juFormat.M2;
    M[1] = Date.juFormat.M1;

    function _thisModel(i, ov, utc) {
        this.i = i;
        this.ov = ov;
        this.utc = utc || false;
    }
    var replacements = [];

    function getValue(t) {
        if (t.indexOf("d") > -1) {
            return t.length == 0 ? "" : d[t.length][t.length < 3 ? _this.getDate() : _this.getDay()];
        } else if (t.indexOf("M") > -1) {
            return t.length == 0 ? "" : M[t.length][_this.getMonth()];
        } else if (t.indexOf("y") > -1) {
            var y = _this.getFullYear().toString();
            return t.length == 0 ? "" : y.slice(4 - t.length, 4);
        } else if (t.indexOf("H") > -1) {
            var H = _this.getHours().toString();
            return t.length == 0 ? "" : H.length == 1 && t.length > 1 ? "0" + H : H;
        } else if (t.indexOf("h") > -1) {
            var h = (_this.getHours() % 12).toString();
            return t.length == 0 ? "" : h.length == 1 && t.length > 1 ? "0" + h : h;
        } else if (t.indexOf("m") > -1) {
            var m = _this.getMinutes().toString();
            return t.length == 0 ? "" : m.length == 1 && t.length > 1 ? "0" + m : m;
        } else if (t.indexOf("s") > -1) {
            var s = _this.getSeconds().toString();
            return t.length == 0 ? "" : s.length == 1 && t.length > 1 ? "0" + s : s;
        } else if (t.indexOf("z") > -1) {
            var z = _this.getMilliseconds()
            return t.length == 0 ? "" : t.length < 3 ? (z / Math.pow(10, (3 - t.length))).toPointedString(0) : z;
        } else if (t.indexOf("w") > -1) {
            var w = _this.getWeek().toString();
            return t.length == 0 ? "" : w.length == 1 && t.length > 1 ? "0" + w : w;
        } else {
            return t;
        }
    }

    function getUtcValue(t) {
        if (t.indexOf("d") > -1) {
            return t.length == 0 ? "" : d[t.length][t.length < 3 ? _this.getUTCDate() : _this.getUTCDay()];
        } else if (t.indexOf("M") > -1) {
            return t.length == 0 ? "" : M[t.length][_this.getUTCMonth()];
        } else if (t.indexOf("y") > -1) {
            var y = _this.getUTCFullYear().toString();
            return t.length == 0 ? "" : y.slice(4 - t.length, 4);
        } else if (t.indexOf("H") > -1) {
            var H = _this.getUTCHours().toString();
            return t.length == 0 ? "" : H.length == 1 && t.length > 1 ? "0" + H : H;
        } else if (t.indexOf("h") > -1) {
            var h = (_this.getUTCHours() % 12).toString();
            return t.length == 0 ? "" : h.length == 1 && t.length > 1 ? "0" + h : h;
        } else if (t.indexOf("m") > -1) {
            var m = _this.getUTCMinutes().toString();
            return t.length == 0 ? "" : m.length == 1 && t.length > 1 ? "0" + m : m;
        } else if (t.indexOf("s") > -1) {
            var s = _this.getUTCSeconds().toString();
            return t.length == 0 ? "" : s.length == 1 && t.length > 1 ? "0" + s : s;
        } else if (t.indexOf("z") > -1) {
            var z = _this.getUTCMilliseconds()
            return t.length == 0 ? "" : t.length < 3 ? (z / Math.pow(10, (3 - t.length))).toPointedString(0) : z;
        } else if (t.indexOf("w") > -1) {
            var w = _this.getWeek().toString();
            return t.length == 0 ? "" : w.length == 1 && t.length > 1 ? "0" + w : w;
        } else {
            return t;
        }
    }

    // dolar içinde kalan kısma format karekterlerine bakılmaz direk çıktı olarak aynen basar.
    var dolars = [];
    var startIndex = format.indexOf('$:');
    var lastIndex = format.indexOf(":$");
    var index = 1;
    while (startIndex > -1 && lastIndex > -1) {
        var _ov = format.substr(startIndex, lastIndex - startIndex + 2);
        dolars.push(new _thisModel(-index, _ov.replace('$:', '').replace(":$", '')));
        format = format.replace(_ov, '{-' + index + '}');
        index++;
        startIndex = format.indexOf('$:');
        lastIndex = format.indexOf(":$");
    }

    //string UTC değerine çevrilir
    function getUtc(tm) {
        replace = [];
        var result = "";
        for (var i = 0; i < tm.length; i++) {
            if (replace.length == 0) {
                replace.push(new _thisModel(i, tm[i]));
                result += '{' + i + '}';
            } else {
                var index = replace.length - 1;
                if (replace[index].ov.indexOf(tm[i]) > -1) {
                    replace[index].ov += tm[i];
                } else {
                    replace.push(new _thisModel(i, tm[i]));
                    result += '{' + i + '}';
                }
            }
        }

        replace.forEach(function (r) {
            result = result.insert({
                i: r.i,
                s: getUtcValue(r.ov)
            });
        });
        return result;
    }

    //UTC içinde kalan kısmın UTC değerini dolar değişkeni gibi dönderir
    var startIndex = format.indexOf('UTC(');
    var lastIndex = format.indexOf(')', startIndex);
    while (startIndex > -1) {
        var _utc = format.substring(startIndex, lastIndex + 1);
        utc = getUtc(new _thisModel(-index, _utc.replace('UTC(', '').replace(")", '')).ov);
        format = format.replace(_utc, '{-' + index + '}');
        dolars.push(new _thisModel(-index, utc));
        index++;

        startIndex = format.indexOf('UTC(');
        lastIndex = format.indexOf(')', startIndex);
    }

    var result = "";
    for (var i = 0; i < format.length; i++) {
        if (replacements.length == 0) {
            replacements.push(new _thisModel(i, format[i]));
            result += '{' + i + '}';
        } else {
            var index = replacements.length - 1;
            if (replacements[index].ov.indexOf(format[i]) > -1) {
                replacements[index].ov += format[i];
            } else {
                replacements.push(new _thisModel(i, format[i]));
                result += '{' + i + '}';
            }
        }
    }

    replacements.forEach(function (r) {
        result = result.insert({
            i: r.i,
            s: getValue(r.ov)
        });
    });

    dolars.forEach(function (r) {
        result = result.insert({
            i: r.i,
            s: r.ov
        });
    });

    return result;
}

Date.prototype.getTicks = function () {
    return (this.getTime() * 10000) + 621355968000000000;
}

Date.prototype.getPreviousDay = function (days) {
    days = days || 1;

    return (this.getTicks() - (days * 864000000000)).getDate();
}

Date.prototype.getNextDay = function (days) {
    days = days || 1;

    return (this.getTicks() + (days * 864000000000)).getDate();
}

Date.prototype.getPreviousDates = function (days) {
    var arr = [];
    days = days || 0;
    for (var i = 1; i < days; i++) {
        arr.push(this.getPreviousDay(days - i));
    }
    arr.push(this);
    return arr;
}

Date.prototype.getNextDates = function (days) {
    var arr = [];
    days = days || 0;
    arr.push(this);
    for (var i = 1; i < days; i++) {
        arr.push(this.getNextDay(i));
    }
    return arr;
}

Date.prototype.getPreviousMonths = function (months) {
    var arr = [];
    months = months || 0;

    var month = this.getMonth();
    var year = this.getFullYear();
    var diff = 1;

    var date = new Date(this);
    for (var i = 1; i < months; i++) {
        if (date.getMonth() == 0) {
            date.setFullYear(date.getFullYear() - 1);
            date.setMonth(11);
            diff = 0;
        }

        date.setMonth(date.getMonth() - diff);
        diff = 1;
        arr.push(new Date(date));
    }
    arr = arr.order();

    arr.push(this);
    return arr;
}

Date.prototype.getNextMonths = function (months) {
    var arr = [];
    months = months || 0;

    var month = this.getMonth();
    var year = this.getFullYear();
    var diff = 1;

    arr.push(this);

    var date = new Date(this);
    for (var i = 1; i < months; i++) {
        if (date.getMonth() == 11) {
            date.setFullYear(date.getFullYear() + 1);
            date.setMonth(0);
            diff = 0;
        }

        date.setMonth(date.getMonth() + diff);
        diff = 1;
        arr.push(new Date(date));
    }

    return arr;
}

Date.prototype.getTimezoneDifference = function () {
    return this.getHours() - this.getUTCHours();
}


String.prototype.addTimezoneDifferenceString = function () {
    var result = "";
    var servertime_start = this.substr(0, 2);
    var servertime_end = this.substr(8, 2);


    var numberStart = (Number(servertime_start) + new Date().getTimezoneDifference()).toString();
    if (numberStart.length == 1)
        numberStart = "0" + numberStart;

    var numberEnd = (Number(servertime_end) + new Date().getTimezoneDifference()).toString();
    if (numberEnd.length == 1)
        numberEnd = "0" + numberEnd;

    result = (numberStart + this.substr(2, 6) + numberEnd + this.substr(10, 3));

    return result;

}

String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
}

String.prototype.getUTCHourMin = function () {
    var value = String(this);
    var hourMin = value.split(":");

    try {
        //var date = new Date("1970-01-01T" + value + ":00.000");
        var date = new Date();
        date.setHours(hourMin[0], hourMin[1]);

        if (date.toString() != 'Invalid Date') {
            return addZero(date.getUTCHours()) + ':' + addZero(date.getUTCMinutes());
        } else
            return value
    } catch (e) {
        return value
    }

    function addZero(val) {
        if (val < 10) {
            val = "0" + val;
        }
        return val;
    }
}

String.prototype.addUTCHourMin = function () {
    var value = String(this);
    try {
        var date = new Date("1970-01-01T" + value + ":00.000Z");
        if (date.toString() != 'Invalid Date') {
            return addZero(date.getHours().toString()) + ':' + addZero(date.getMinutes().toString());
        } else
            return value
    } catch (e) {
        return value
    }

    function addZero(val) {
        if (val < 10) {
            val = "0" + val;
        }
        return val;
    }
}

String.prototype.toPointedString = function () {
    return String(this);
}

String.prototype.htmlCodeToChar = function () {
    var result = window.String(this);

    result = result.replace(/&quot;/g, '"');
    result = result.replace(/&#252;/g, 'ü');
    result = result.replace(/&#220;/g, 'Ü');
    result = result.replace(/&#246;/g, 'ö');
    result = result.replace(/&#214;/g, 'Ö');
    result = result.replace(/&#231;/g, 'ç');
    result = result.replace(/&#199;/g, 'Ç');
    result = result.replace(/&#x3D;/g, '=');
    result = result.replace(/&amp;/g, '&');
    result = result.replace(/\t/g, '\\t');

    return result;
}

String.prototype.stringToJson = function () {
    var result = window.String(this);
    try {
        result = window.JSON.parse(result.htmlCodeToChar());

        return result;
    } catch (e) {
        return result;
    }
}

String.prototype.insert = function () {
    var string = this;
    for (var i = 0; i < arguments.length; i++) {
        if (arguments[i].__proto__ == Object.prototype) {
            if (string.indexOf('{' + arguments[i].i + '}') > -1)
                string = string.split('{' + arguments[i].i + '}').join(arguments[i].s);
            else if (arguments[i].e == undefined || arguments[i].e == true)
                string += '\n' + arguments[i].s;
        } else {
            if (string.indexOf('{0}') > -1)
                string = string.replace('{0}', arguments[i]);
            else
                string += '\n' + arguments[i];
        }
    }
    return String(string);
}

String.prototype.stringToDateTime = function (dIndex, MIndex, yIndex, hIndex, mIndex, sIndex, hour) {
    var _this = String(this);
    var pm = _this.indexOf('PM') > -1;

    var t = {
        d: 0,
        M: 0,
        y: 0,
        h: 0,
        m: 0,
        s: 0,
    };

    function get(p) {
        if (p == 'h') {
            t[p] = t[p] + (pm ? 12 : 0);
        }

        if (t[p].toString().length < 2) {
            t[p] = '0' + t[p];
        }
        return t[p];
    }

    var indexes = [];
    indexes[(dIndex === window.undefined ? -1 : dIndex)] = 'd';
    indexes[(MIndex === window.undefined ? -1 : MIndex)] = 'M';
    indexes[(yIndex === window.undefined ? -1 : yIndex)] = 'y';
    indexes[(hIndex === window.undefined ? -1 : hIndex)] = 'h';
    indexes[(mIndex === window.undefined ? -1 : mIndex)] = 'm';
    indexes[(sIndex === window.undefined ? -1 : sIndex)] = 's';

    var i = 0;
    Array.prototype.forEach.call(_this, function (char) {
        if (!char.match('[0-9]')) {
            i++;
        } else {
            t[indexes[i]] = (t[indexes[i]] * 10 + Number(char));
        }
    });
    var result = new Date(get('y') + '-' + get('M') + '-' + get('d') + ' ' + get('h') + ':' + get('m') + ':' + get('s'))
    result.setHours(result.getHours() - (hour || 0));
    return result;
}

String.prototype.replaceByIndex = function (startIndex, endIndex, replacement) {
    var str = String(this);
    var before = str.substr(0, startIndex);
    var after = str.substr(endIndex);
    return before + replacement + after;
}

String.prototype.htmlEncode = function () {
    var str = String(this);
    return $('<div/>').text(str).html();
}

String.prototype.htmlDecode = function () {
    var str = String(this);
    return $('<div/>').html(str).text();
}

String.prototype.clearQuot = function () {
    var str = String(this);
    return str.replaceAll('"', "&quot;");
}

String.prototype.insertObject = function (params, level, ignore) {
    level = level || 0;
    try {
        var str = String(this);
        // eğer string varsa işlemler yapılmaya başlanır.
        if (str) {
            var templates = [];
            var scripts = [];

            // script tag'i içinde olan hiçbir string eval işlemlerinden geçirilmeyecek.
            while (str.indexOf('<script') > -1) {
                var sIndex = str.indexOf('<script');
                var lIndex = str.indexOf('</script>') + 9 - str.indexOf('<script');

                var script = str.substr(sIndex, lIndex);
                str = str.replace(script, '#script#');
                scripts.push(script);
            }

            // template tag'i içinde olan hiçbir string eval işlemlerinden geçirilmeyecek.
            while (str.indexOf('<template') > -1) {
                var sIndex = str.indexOf('<template');
                var lIndex = str.indexOf('</template>') + 11 - str.indexOf('<template');

                var template = str.substr(sIndex, lIndex);
                str = str.replace(template, '#template#');
                templates.push(template);
            }

            // ju-script attribut'ü olan taglar $: :$ şekline dönüştürülerek eval işlemine sokulacak.
            while (str.indexOf('ju-script="') > -1) {
                var substr = str.substr(str.indexOf('ju-script="') + 11);
                substr = substr.substr(0, substr.indexOf('"'));
                str = str.replace('ju-script="' + substr + '"', '$:' + substr + ':$');
            }

            // eğer ki juscript syntax'ı için "$:" değişim anahtar kelimeleri yoksa işlem yaptırılmaz.
            if (str.indexOf('$:') > -1 && str.indexOf('$:') < str.indexOf(':$')) {
                var tempparam = replaceEvalResultOfParams(params);

                // parametreler tanımlanır. eval edilirken kullanılmak üzere hazırlanır.
                function objectToStringVars(str, params) {
                    var vars = '';
                    for (var p in params) {
                        if (str.indexOf(p) > -1) {
                            if (typeof (params[p]) == 'string' && (new RegExp(/(<[\w]+)/).test(params[p]) || params[p].indexOf("\"") > -1) && !ignore)  //&& new RegExp(/(<[\w]+)/).test(params[p])
                            {
                                vars += 'var ' + p + ' = this.' + p + ';';
                            }
                            else
                                vars += 'var ' + p + ' = this.' + p + ';';
                        }
                    }
                    return vars;
                }

                // stringlerin eval edildiği yerdir.
                // error çıkan bölüm burada sadece kendisi ile korunmuş olur. başka $: :$ bloklarını etkilemez.
                function replaceEvalResult(str, params, vars, valued) {
                    var $errsindex = 0,
                        $erreindex = 0,
                        index = str.indexOf('$:', $errsindex);
                    while (index > -1) {
                        var sIndex = index;
                        var eIndex = str.indexOf(':$', $erreindex || sIndex + 2);

                        var fn = str.substr(sIndex + 2, eIndex - 2 - sIndex);
                        var result = (function () {
                            try {
                                eval(vars);
                                return eval(fn);
                            } catch (e) {
                                $errsindex = sIndex + 1;
                                $erreindex = eIndex + 1;
                                return '$:' + fn + ':$';
                            }
                        }).apply(params);

                        if (valued)
                            return result;
                        else {
                            str = str.replaceByIndex(sIndex, eIndex + 2, result);
                            index = str.indexOf('$:', $errsindex);
                        }
                    }
                    return str;
                }

                // eğer ki eval işlemleri yapılmadan önce $: :$ içinde bir tane daha 
                // $: $: :$ :$ açılmışsa, iç kısımdakiler silinir.
                function clearDoubleSyntax(str) {
                    var $index = str.indexOf('$:');
                    while ($index > -1 && $index + 3 < str.length) {
                        var removed = 0;
                        for (var i = $index + 2; i < str.length; i++) {
                            if (str[i] + str[i + 1] == ':$') {
                                if (removed == 0) {
                                    $index = i + str.substr(i).indexOf('$:');
                                    break;
                                } else {
                                    str = str.replaceByIndex(i, i + 2, '');
                                    removed--;
                                }
                            }
                            if (str[i] + str[i + 1] == '$:') {
                                removed++;
                                str = str.replaceByIndex(i, i + 2, '');
                            }
                        }
                        if (i >= str.length)
                            $index = str.length;
                    }
                    return str;
                }

                // parametre içinde $: :$ syntax varsa bunlar'ın sonuçları ile değiştirilir.
                function replaceEvalResultOfParams(params) {
                    params = juInsertObject({}, params);
                    for (var p in params) {
                        if (typeof (params[p]) == 'string' && params[p].indexOf('$:') > -1 && params[p].indexOf('$:') < params[p].indexOf(':$')) {
                            params[p] = replaceEvalResult(params[p], params, objectToStringVars(params[p], params), true);
                        } else if (typeof (params[p]) == 'string' && (new RegExp(/(<[\w]+)/).test(params[p]) || params[p].indexOf("\"") > -1) && !ignore) {
                            params[p] = params[p].htmlEncode().clearQuot();
                        }
                    }
                    return params;
                }

                // eval edilecek bölüm içinde html var ise, html olarak belirtilen bölümlerin başına sonuna string işareti konulur.
                function replaceHtml(str) {
                    var index = str.indexOf('#html#');
                    if (index > -1) {
                        var ov = str.substring(index, str.indexOf('#html end#') + 10);
                        var tempstr = ov.replaceAll('(( )*<)', '<').replaceAll('(( )*(#html))', '#html').replaceAll('\n', '').replaceAll('#html end#', '\'').replaceAll('#html#', '\'').replaceAll('"', '\\"');
                        var nv = tempstr.indexOf('$:') > -1 && tempstr.indexOf('$:') < tempstr.indexOf(':$') ? replaceEvalResult(tempstr, tempparam, objectToStringVars(tempstr, tempparam)) : tempstr;
                        str = str.replaceAll(ov, nv);
                        str = replaceHtml(str);
                    }
                    return str;
                }

                str = replaceEvalResult(clearDoubleSyntax(str.indexOf('#html#') > -1 ? replaceHtml(str) : str), tempparam, objectToStringVars(str, tempparam));
            }

            // template tagleri tekrar düzenlenir.
            while (str.indexOf('#template#') > 0) {
                str = str.replace('#template#', templates[0]);
                templates.splice(0, 1);
            }

            // script tagleri tekrar düzenlenir.
            while (str.indexOf('#script#') > 0) {
                if (scripts[0].indexOf('.call(setId(') > -1) {
                    var scriptId = ''.getGuid();
                    var old_id = null;
                    if (scripts[0].indexOf('<script id="') > -1) {
                        old_id = scripts[0].substring(12, 48);
                        scripts[0] = scripts[0].replace(scripts[0].substring(0, 49), '<script id="' + scriptId + '"');

                    }
                    else
                        scripts[0] = scripts[0].replace('<script', '<script id="' + scriptId + '"');

                    if (scripts[0].indexOf('.call(setId({') > -1) {
                        if (scripts[0].indexOf('.call(setId({id:"') > -1) {
                            scripts[0] = scripts[0].replace(old_id, scriptId);
                        }
                        else
                            scripts[0] = scripts[0].replace('.call(setId({', '.call(setId({id:"' + scriptId + '",');
                    }
                    else
                        scripts[0] = scripts[0].replace('.call(setId(', '.call(setId({id:"' + scriptId + '"}');
                }
                str = str.replace('#script#', scripts[0]);
                scripts.splice(0, 1);
            }
        }

        // level'e göre jquery nesne veya string olarak sonuç üretilir.
        if (level > 0)
            return str;
        else {
            var div = document.createElement('div');
            $(div).html(str);
            var element = $(div).children();
            element.data({
                insertObject: params
            });
            return element;
        }
    } catch (e) {
        return String(this);
    }
}

String.prototype.regExpEscape = function () {
    var str = String(this);
    return str.split('').map(function (c) {
        return ['.', '\\', '+', '*', '?', '[', '^', ']', '$', '(', ')', '{', '}', '=', '!', '<', '>', '|', ':', '-'].indexOf(c) > -1 ? '\\' + c : c;
    }).join('');
}

String.prototype.getGuid = function () {
    var lut = [];
    for (var i = 0; i < 256; i++) {
        lut[i] = (i < 16 ? '0' : '') + i.toString(16);
    }
    var d0 = Math.random() * 0xffffffff | 0;
    var d1 = Math.random() * 0xffffffff | 0;
    var d2 = Math.random() * 0xffffffff | 0;
    var d3 = Math.random() * 0xffffffff | 0;
    return (lut[d0 & 0xff] + lut[d0 >> 8 & 0xff] + lut[d0 >> 16 & 0xff] + lut[d0 >> 24 & 0xff] + '-' +
        lut[d1 & 0xff] + lut[d1 >> 8 & 0xff] + '-' + lut[d1 >> 16 & 0x0f | 0x40] + lut[d1 >> 24 & 0xff] + '-' +
        lut[d2 & 0x3f | 0x80] + lut[d2 >> 8 & 0xff] + '-' + lut[d2 >> 16 & 0xff] + lut[d2 >> 24 & 0xff] +
        lut[d3 & 0xff] + lut[d3 >> 8 & 0xff] + lut[d3 >> 16 & 0xff] + lut[d3 >> 24 & 0xff]).toLowerCase();
}

String.prototype.replaceAll = function (text1, text2) {
    var str = '';
    try {
        str = String(this);
        str = str.replace(new RegExp(text1.regExpEscape(), "gm"), text2);
    } catch (e) {
        str = String(this);
    }
    return str;
}

String.prototype.toLowerTurkish = function () {
    var _this = String(this);
    try {
        _this = _this.replace(/ı/g, "i");
        _this = _this.replace(/ğ/g, "g");
        _this = _this.replace(/ü/g, "u");
        _this = _this.replace(/ş/g, "s");
        _this = _this.replace(/ö/g, "o");
        _this = _this.replace(/ç/g, "c");
        _this = _this.replace(/İ/g, "I");
        _this = _this.replace(/Ğ/g, "G");
        _this = _this.replace(/Ü/g, "U");
        _this = _this.replace(/Ş/g, "S");
        _this = _this.replace(/Ö/g, "O");
        _this = _this.replace(/Ç/g, "C");
        _this = _this.replace(/[^a-zA-Z0-9 ]/g, "");

        return _this.toLocaleLowerCase();
    } catch (e) {
        return _this;
    }
}

Array.prototype.intersection = function (list) {
    var _this = this;
    var result = [];
    if (_this.length < list.length)
        result = _this.filter(function (item) {
            return list.indexOf(item) > -1;
        });
    else
        result = list.filter(function (item) {
            return _this.indexOf(item) > -1;
        });

    return result;
}

Array.prototype.juAggregation = function (obj) {
    var measures = obj.measures || [];
    var fields = obj.fields || [];
    var filters = obj.filters || [];

    if (this.length > 0) {
        function setMeasureByScript(oldItem, item) {
            measures.forEach(function (measure) {
                try {
                    oldItem[measure] += (item[measure] || 0);
                } catch (e) {
                    oldItem[measure] = 0;
                }
            });
        }

        var result = [];
        this.forEach(function (item) {
            var exist = filters.length == 0 || filters.filter(function (filter) {
                return filter.values.map(function (_value) {
                    return _value == item[filter.field];
                }).indexOf(true) > -1;
            }).length == filters.length;

            if (exist) {
                var oldItem = null;
                if (fields.length > 0)
                    oldItem = result[result.map(function (rItem) {
                        var rv = '';
                        var dv = '';
                        fields.forEach(function (field) {
                            rv += rItem[field];
                            dv += item[field];
                        });
                        return rv == dv;
                    }).indexOf(true)];
                else
                    oldItem = result[0];

                if (oldItem) {
                    setMeasureByScript(oldItem, item);
                } else {
                    var newItem = {};
                    fields.forEach(function (field) {
                        newItem[field] = item[field];
                    });
                    measures.forEach(function (measure) {
                        newItem[measure] = 0;
                    });
                    setMeasureByScript(newItem, item);

                    result.push(newItem);
                }
            }
        });
        return result;
    } else {
        return [];
    }
}

Array.prototype.max = function () {
    var maxValue = 0;
    for (var i = 0; i < this.length; i++) {
        if (this[i] && maxValue < this[i]) {
            maxValue = this[i];
        }
    }
    return maxValue;
};

Array.prototype.maxByProp = function (prop) {
    var maxValue = 0;
    var maxObj = {};
    var valued = true;
    for (var i = 0; i < this.length; i++) {
        if (valued && this[i][prop]) {
            maxValue = this[i][prop];
            maxObj = this[i];
            valued = false;
        }

        if (this[i][prop] && maxValue < this[i][prop]) {
            maxValue = this[i][prop];
            maxObj = this[i];
        }
    }
    return maxObj;
}

Array.prototype.min = function () {
    var minValue = Number.MAX_VALUE;
    for (var i = 0; i < this.length; i++) {
        if (this[i] && minValue > this[i]) {
            minValue = this[i];
        }
    }
    return minValue;
};

Array.prototype.minByProp = function (prop) {
    var minValue = Number.MAX_VALUE;
    var minObj = {};
    var valued = true;
    for (var i = 0; i < this.length; i++) {
        if (valued && this[i][prop]) {
            minValue = this[i][prop];
            minObj = this[i];
            valued = false;
        }

        if (this[i][prop] && minValue > this[i][prop]) {
            minValue = this[i][prop];
            minObj = this[i];
        }
    }
    return minObj;
}

Array.prototype.total = function () {
    var total = 0;
    for (var i = 0; i < this.length; i++) {
        total += this[i];
    }
    return total;
}

Array.prototype.totalByProp = function (prop) {
    var total = 0;
    for (var i = 0; i < this.length; i++) {
        if (this[i][prop])
            total += this[i][prop];
    }
    var obj = {};
    obj[prop] = total;
    return obj;
}

Array.prototype.average = function () {
    var total = this.total();
    var average = total / this.length;
    return average;
}

Array.prototype.averageByProp = function (prop) {
    var _this = this;
    var props = [];
    if (arguments.length > 1) {
        props = Array.prototype.map.call(arguments, function (item) {
            return item;
        });
    } else {
        props.push(prop);
    }
    var obj = {};
    props.forEach(function (prop) {
        var total = _this.totalByProp(prop)[prop];
        var average = total / _this.length;
        obj[prop] = average;
    });
    return obj;
}

Array.prototype.take = function (indexes) {
    indexes = indexes || 1;
    var result = null;
    try {
        // Eğer ki parametre tek sayı ise o sıradakini dön.
        if (indexes.constructor.prototype == window.Number.prototype && indexes > 0) {
            result = this[indexes - 1];
        }
        // eğer ki parametre int array ise, tüm değerleri liste olarak dön.
        else if (indexes.constructor.prototype == window.Array.prototype && indexes.length > 0) {
            result = [];
            for (var i = 0; i < this.length; i++) {
                if (indexes.indexOf(i + 1) > -1) {
                    result.push(this[i]);
                }
            }
        } else {
            result = null;
        }
    } catch (e) {
        result = null;
    }
    return result;
}

Array.prototype.takeTop = function (top) {
    var _this = this;
    var correct = top && top > 0;
    try {
        var result = [];
        if (correct) {
            for (var i = 0; i < top && i < _this.length; i++) {
                result.push(_this[i]);
            }
        } else {
            result = _this;
        }
        return result;
    } catch (e) {
        return _this;
    }
}

Array.prototype.skipTake = function (skip, take) {
    var _this = this;
    var correct = typeof (skip) == 'number' && take && skip > -1 && take > 0;
    try {
        var result = [];
        if (correct) {
            for (var i = skip; i < skip + take && i < _this.length; i++) {
                result.push(_this[i]);
            }
        } else {
            result = _this;
        }
        return result;
    } catch (e) {
        return _this;
    }
}

Array.prototype.orderBy = function (prop, type, datatype) {
    type = type || 'asc', datatype = datatype || [];
    try {
        var capitalized = datatype && typeof (datatype) != 'object' ? datatype : false;

        var ordered = [];
        var booleanArr = [];
        var numberArr = [];
        var stringArr = [];

        if (typeof (datatype) == 'object' && datatype.length == 0 && this.length > 0) {
            this.forEach(function (item) {
                if (item[prop] != null) {
                    datatype.push(item[prop].constructor);

                    if (typeof (item[prop]) == 'number')
                        numberArr.push(item);
                    else if (typeof (item[prop]) == 'string')
                        stringArr.push(item);
                    else if (typeof (item[prop]) == 'boolean')
                        booleanArr.push(item);
                }
            });

            datatype = datatype.distinct();
            if (datatype.length == 1)
                datatype = datatype[0];
        }

        function stringSort(arr) {
            return arr.sort(function (a, b) {
                var _a = a[prop] || '';
                var _b = b[prop] || '';

                _a = _a.toLowerTurkish();
                _b = _b.toLowerTurkish();

                if (type === 'asc') {
                    if (_a < _b) return -1
                    if (_a > _b) return 1
                } else if (type === 'desc') {
                    if (_a > _b) return -1
                    if (_a < _b) return 1
                }

                return 0
            });
        }

        function anySort(arr, datatype) {
            return arr.sort(function (a, b) {
                var _a = a[prop] == undefined ? datatype() : a[prop];
                var _b = b[prop] == undefined ? datatype() : b[prop];

                if (capitalized) {
                    _a = datatype(_a);
                    _b = datatype(_b);
                }

                if (type === 'asc') {
                    if (_a < _b) return -1
                    if (_a > _b) return 1
                } else if (type === 'desc') {
                    if (_a > _b) return -1
                    if (_a < _b) return 1
                }
                return 0
            });
        }

        if (typeof (datatype) == 'object') {
            booleanArr = anySort(booleanArr, Boolean);
            numberArr = anySort(numberArr, Number);
            stringArr = stringSort(stringArr);

            if (type == 'asc')
                ordered = ordered.concat(booleanArr).concat(numberArr).concat(stringArr);
            else
                ordered = ordered.concat(stringArr).concat(numberArr).concat(booleanArr);
        } else {
            if (datatype == String) {
                ordered = stringSort(this);
            } else {
                ordered = anySort(this, datatype);
            }
        }
        return ordered;
    } catch (e) {
        return this;
    }
}

Array.prototype.orderByProps = function _orderByProps() {
    var _this = this;
    var p = "$$_ju_temp_index";

    _this.forEach(function (item) {
        item[p] = 0;
    });
    var result = _this;

    var l = _this.length;
    var c = arguments.length;

    Array.prototype.forEach.call(arguments, function (prop, i) {
        var j = 0;
        result = result.orderBy(prop);
        result.forEach(function (item, k) {
            var n = c - i - 1;
            var prev = window.$ju.sequentialForceTotal(l, n - 1);
            var cur = window.$ju.force(l, n) / l * (j + 1);
            item[p] += (prev + cur);

            if (result[k + 1] && item[prop] != result[k + 1][prop])
                j++;
        });
    });

    result = result.orderBy(p);
    result.forEach(function (item) {
        delete item[p];
    });
    return result;
}

Array.prototype.order = function (type, datatype) {
    type = type || 'asc';
    var capitalized = datatype ? datatype.capitalize() : false;
    var dataType = capitalized ? window[capitalized].prototype : this.length > 0 ? this[0].constructor.prototype : window.Object.prototype;
    try {
        var ordered = [];
        if (dataType == window.String.prototype) {
            ordered = this.sort(function (a, b) {
                var _a = a.toLowerTurkish();
                var _b = b.toLowerTurkish();
                if (type === 'asc') {
                    if (_a < _b) return -1
                    if (_a > _b) return 1
                } else if (type === 'desc') {
                    if (_a > _b) return -1
                    if (_a < _b) return 1
                }
                return 0
            });
        } else {
            ordered = this.sort(function (a, b) {
                var _a = capitalized ? window[capitalized](a) : a;
                var _b = capitalized ? window[capitalized](b) : b;
                if (type === 'asc') {
                    if (_a < _b) return -1
                    if (_a > _b) return 1
                } else if (type === 'desc') {
                    if (_a > _b) return -1
                    if (_a < _b) return 1
                }
                return 0
            });
        }

        return ordered;
    } catch (e) {
        return this;
    }
}

Array.prototype.toPropList = function (prop) {
    try {
        var result = [];
        for (var i = 0; i < this.length; i++) {
            for (var p in this[i]) {
                if (p == prop) {
                    result.push(this[i][p]);
                    break;
                }
            }
        }
        return result;
    } catch (e) {
        return [];
    }
}

Array.prototype.custom = function (script) {
    try {
        var result = '';
        var data = this;

        result = eval(script);

        return result;
    } catch (e) {
        return '';
    }
}

Array.prototype.distinct = function () {
    try {
        var result = [];
        for (var i = 0; i < this.length; i++) {
            if (result.indexOf(this[i]) == -1) {
                result.push(this[i]);
            }
        }
        return result;
    } catch (e) {
        return [];
    }
}

Array.prototype.notNull = function () {
    try {
        var result = [];
        for (var i = 0; i < this.length; i++) {
            if (this[i]) {
                result.push(this[i]);
            }
        }
        return result;
    } catch (e) {
        return [];
    }
}

Array.prototype.getByProp = function (prop, value) {
    if (value instanceof RegExp) {
        return this.filter(function (e) {
            return value.test(e[prop]);
        })[0];
    } else {
        return this.filter(function (e) {
            return e[prop] == value;
        })[0];
    }
}

Array.prototype.removeByProp = function (prop, value) {
    var _this = this;
    var result = _this.filter(function (item) {
        return item[prop] != value;
    });
    return result;
}

Array.prototype.remove = function (value) {
    var _this = this;
    var result = _this.filter(function (item) {
        return item != value;
    });
    return result;
}

Array.prototype.juFilter = function (selector) {
    var result = this.filter(function (item) {
        return eval(selector);
    });
    return result;
}

Array.prototype.juTrim = function () {
    var result = [];
    this.forEach(function (item) {
        if (item.trim)
            result.push(item.trim());
    });

    return result;
}

Array.prototype.juReplace = function (value1, value2) {
    var array = this.map(function (item) {
        if (item == value1) {
            return value2;
        }
        return item;
    });
    return array;
}

Array.prototype.juGetSameLikeSingle = function () {
    var _this = this;
    var result = _this.filter(function (item, i) {
        return _this.indexOf(item) == i;
    });
    return result;
}

detectUndefineMembers = function (data, columns) {
    Object.keys(columns).forEach(function (key) {
        if (data[columns[key]] == null)
            data[columns[key]] = window.viewModel.undefinedColumn;
    });

    return data;
}
if (typeof toastr != "undefined") {
    var toastrSuccess = toastr.success;
    toastr.success = function (message) {
        if (message)
            toastrSuccess(message);
    }

    var toastrError = toastr.error;
    toastr.error = function (message) {
        if (message)
            toastrError(message);
    }
}


jQuery.prototype.insertObject = function (obj, level, ignore) {
    obj = obj || {};
    ignore = typeof ignore == 'boolean' ? ignore : false;
    try {
        return this.html().trim().htmlCodeToChar().insertObject(obj, level, ignore);
    } catch (e) {
        if (level > 0)
            return '';
        else
            return $('');
    }
};

var $jQueryFind = jQuery.prototype.find;
jQuery.prototype.find = function () {
    var _this = this;
    var args = arguments;

    function rtrnFn() {
        return $jQueryFind.apply(_this, args);
    }
    if (_this.is('[ju-container]')) {
        var result = rtrnFn();
        if (result.length < 2) {
            return result;
        } else {
            return $(Array.prototype.filter.call(result, function (e) {
                return $(e).closest('[ju-container]').is(_this);
            }));
        }
    } else
        return rtrnFn()
}

jQuery.fn.scrollEnd = function (callback, timeout) {
    $(this).scroll(function () {
        var $this = $(this);
        if ($this.data('scrollTimeout')) {
            clearTimeout($this.data('scrollTimeout'));
        }
        $this.data('scrollTimeout', setTimeout(callback, timeout));
    });
};

jQuery.fn.juToggleGroup = function (obj) {
    var $this = $(this),
        toggleoffClass = "btn-default",
        obj = obj || {};
    var changed = obj.changed,
        _default = obj.default;

    function change($btn) {
        var toggleonClass = $btn.attr('toggle-on');

        $btn.parent().children(".btn").each(function (i, sibling) {
            var $sibling = $(sibling);
            var stoggleonClass = $sibling.attr('toggle-on');
            if (stoggleonClass)
                $sibling.removeClass(stoggleonClass).addClass(toggleoffClass);
            $sibling.removeClass('active');
        });

        if (toggleonClass)
            $btn.removeClass(toggleoffClass).addClass(toggleonClass);
        $btn.addClass('active');
    }

    if (_default) {
        var $default = null;
        if (typeof (_default) == 'string')
            $default = $this.children(_default);
        else if (_default.constructor.prototype == jQuery.prototype)
            $default = _default;

        if ($default) {
            change($default);
        }
    }

    $this.children('.btn').off("click").on("click", function (e) {
        var $btn = null;
        if ($(e.target).is('.btn'))
            $btn = $(e.target);
        else
            $btn = $(e.target).closest('.btn');
        change($btn);

        if (changed)
            changed({
                name: $btn.attr('name'),
                role: $btn.attr('role')
            });
    });
}

jQuery.fn.colorBrush = function (obj) {
    obj = obj || {};
    var $this = $(this),
        changeFn = obj.changeFn,
        brushClick = obj.brushClick;

    //Şablon buton grupları belirlenir
    var buttonGroup = '<div class="color-brush">' +
        '<i class="fa fa-paint-brush fa-2x m-r-sm choosable text-navy" value="#1ab394"></i>' +
        '<i class="fa fa-paint-brush fa-2x m-r-sm choosable text-success" value="#1c84c6"></i>' +
        '<i class="fa fa-paint-brush fa-2x m-r-sm choosable text-warning" value="#f8ac59"></i>' +
        '<i class="fa fa-paint-brush fa-2x m-r-sm choosable text-danger" value="#ED5565"></i>' +
        '<i class="fa fa-paint-brush fa-2x m-r-sm choosable text-info" value="#23c6c8"></i>' +
        '<i class="fa fa-paint-brush fa-2x m-r choosable text-muted" value="#888888"></i>' +
        '<i class="fa fa-paint-brush fa-2x text-primary pull-right showcolor hidden" value=""></i>' +
        '</div>'

    //Butonlar JQuery elemntine dönüştürülür
    var jQueryButtons = $(jQuery.parseHTML(buttonGroup));

    //Bu input colorpickera dönüştürülür
    $this.colorpicker();

    //Herhangi bir fırçaya tıklanıldığı durumda butonun değeri alınır ve gösteren fırça ile input değerine verilir
    var _brushClick = function (event) {
        var colorValue = $(event.target).attr('value');

        jQueryButtons.find('.showcolor').removeClass('hidden').css('color', colorValue);
        $this.val(colorValue);
    }

    //Her değişiklikte renk gösteren fırçanın değeri tutulur
    var _changeFn = function (event) {
        jQueryButtons.find('.showcolor').removeClass('hidden').css('color', event.color.toHex());
    }

    //click eventi eğer dışardan bir fonksiyon yoksa yerel ve default fonksiyonu çağırır
    jQueryButtons.find('.choosable').off('click').on('click', function (event) {
        (brushClick || _brushClick)(event);
    })

    //change eventi eğer dışardan bir fonksiyon yoksa yerel ve default fonksiyonu çağırır
    $this.off('changeColor').on('changeColor', function (event) {
        (changeFn || _changeFn)(event)
    });

    //Default değer varsa gösterilir
    if ($this.val()) {
        jQueryButtons.find('.showcolor').removeClass('hidden').css('color', $this.val());
    }

    //JQuery butonlar inputtan önce sibling olarak HTML'e eklenir.
    $this.before(jQueryButtons);
}

jQuery.fn.hpDropdown = function (obj) {
    /* dropdown init function */
    //sourceData source çekilirken gönderilecek olan veridir. Örneğin selected arkada hazırlanacaksa ve önden id gönderilecekse bu id içerisinde gnderilir.
    //oldItems {Value: string, Text: string, Selected: boolean} object array olacaktır.
    //pageableObject: Sayfa tarafından dropdown pageable yapısı için kullanılacak
    //source: Verinin çekileceği external url. Olmadığı zaman static html üzerinden alır
    //enableFiltering: Filtreleme kontrolü.
    //enableCaseInsensitiveFiltering: Case sensitive filtreleme kontrolü.
    //valueField: dış kaydın value olarak kullanılacak field adı.
    //textField: dış kaydın text olarak kullanılacak field adı.
    //orderBy: dış kaydın sıralamada referans alınacak field adı.
    //orderType: "asc" veya "desc",
    //isExternal: boolean. Html veya url üzerinden alınacağını belirtir.
    //allowEmpty: multi seçilmeyen durumlarda seçeneklere yazısı placeholder olan boş bir option atar   
    //init: Build edildiğinden çağırılacak callback fonksiyonu

    var _el = $(this);
    var isMultiple = _el.attr("multiple");
    if (!isMultiple) _el.attr("size", "2");
    var wasInit = false;

    var _pageable = null;
    var _columns = {
        value: "Value",
        text: "Text",
        selected: "Selected"
    }

    var oldItems = obj.oldItems || [],
        pageableObject = obj.pageableObject,
        source = obj.source,
        enableFiltering = obj.filtering,
        enableCaseInsensitiveFiltering = obj.enableCaseInsensitiveFiltering,
        valueField = obj.valueField || _columns.value,
        textField = obj.textField || _columns.text,
        selected = obj.selected || _columns.selected,
        orderBy = obj.orderBy,
        orderType = obj.orderType || "asc",
        filtering = obj.filtering,
        placeholder = obj.placeholder,
        chosen = obj.chosen,
        init = obj.init,
        sourceData = obj.sourceData || {},
        isExternal = obj.isExternal || false,
        allowEmpty = obj.allowEmpty || false;
    optionTemplate = obj.optionTemplate || $($.parseHTML("<template><option value='$: $value :$'>$: $text :$</option></template>")),
        emptyOptionTemplate = $($.parseHTML("<template><option>" + placeholder + "</option></template>")),
        uniqueIdentifier = obj.uniqueIdentifier || "".getGuid();

    //Dropdown yüklenme süresinde eklenecek olan spiner animasyonu
    var spinner = '<div class="sk-spinner sk-spinner-fading-circle">' +
        '<div class="sk-circle1 sk-circle"></div>' +
        '<div class="sk-circle2 sk-circle"></div>' +
        '<div class="sk-circle3 sk-circle"></div>' +
        '<div class="sk-circle4 sk-circle"></div>' +
        '<div class="sk-circle5 sk-circle"></div>' +
        '<div class="sk-circle6 sk-circle"></div>' +
        '<div class="sk-circle7 sk-circle"></div>' +
        '<div class="sk-circle8 sk-circle"></div>' +
        '<div class="sk-circle9 sk-circle"></div>' +
        '<div class="sk-circle10 sk-circle"></div>' +
        '<div class="sk-circle11 sk-circle"></div>' +
        '<div class="sk-circle12 sk-circle"></div>' +
        '</div>';

    var mutiselectObj = {};
    mutiselectObj.maxHeight = 200;
    mutiselectObj.buttonWidth = '100%';
    mutiselectObj.enableFiltering = filtering || false;
    mutiselectObj.enableCaseInsensitiveFiltering = filtering || false;
    mutiselectObj.filterPlaceholder = window.viewModel.multiselectOptions.filterPlaceholder;
    mutiselectObj.nonSelectedText = "";
    mutiselectObj.buttonText = function (options, select) {
        if (options.length === 0) {
            return placeholder;
        } else if (options.length > chosen) {
            return options.length + ' ' + window.viewModel.multiselectOptions.chosenPlaceholder;
        } else {
            var labels = [];
            options.each(function () {
                if ($(this).attr('label') !== undefined) {
                    labels.push($(this).attr('label'));
                } else {
                    labels.push($(this).html());
                }
            });
            return labels.join(', ') + '';
        }
    }
    _el.multiselect(mutiselectObj);
    var dropdown = _el.parent().find('[data-toggle="dropdown"]');
    dropdown.dropdown();

    _el.closest('.bootstrap-multiselect-container').find('*').click(function (e) {
        e.stopPropagation();
    })
    _el.closest('.modal-dialog').bind('click_important', function (e) {
        _el.parent().find('.btn-group').removeClass('open');
    });

    if (isExternal) {
        _el.siblings('.btn-group').find('button').html(spinner);
        if (pageableObject)
            _pageable = getPageableData();
        else {
            getData();
        }
    } else {
        if (wasInit == false && init) {
            wasInit = true;
            init();
        }

        return;
    }

    function insertObject(element, obj, level) {
        obj = juInsertObject(obj, _columns, function (key, injectValue, reference) {
            if (reference[key] === undefined)
                reference[key] = injectValue;
            reference["$" + key] = "$: " + injectValue + " :$";
        });
        return jQuery.prototype.insertObject.call(element, obj, level, _columns);
    }

    //pageable olarak veri çeken functiondır
    function getPageableData() {
        return pageableObject.setConfiguration({
            order: {
                "by": orderBy,
                "type": orderType
            },
            url: source,
            data: sourceData || {},
            success: function (res) {
                setConfiguration(res || []);
            },
            error: function (res) {
                setConfiguration([]);
            }
        });
    }

    //Normal olarak data çeken servistir
    function getData() {
        new juAjax({
            type: 'POST',
            url: source,
            data: sourceData,
            defineId: uniqueIdentifier,
            success: function (response) {
                setConfiguration((response && response.Data) || [])
            },
            error: function (response) {
                setConfiguration([])
            },
        });
    }

    //Verileri Value ve Text formatına çeviren yerdir
    function setConfiguration(res) {
        setDropdown(res.map(function (item) {
            return {
                Value: item[valueField],
                Text: item[textField],
                Selected: item[selected] || false
            };
        }))
    }

    //Dropdownı yapılandıran methottur.
    function setDropdown(list) {
        oldItems = oldItems.concat(list.filter(function (item) {
            return item[_columns.selected];
        }));

        oldItems = oldItems.filter(function (item) {
            return item;
        });

        oldItems.forEach(function (item) {
            var matchedItem = list.getByProp(_columns.value, item[_columns.value]);

            if (!matchedItem) {
                item[_columns.selected] = true;
                list.unshift(item);
            } else {
                matchedItem[_columns.selected] = true;
            }
        });

        var scrollLength = _el.siblings().find('.multiselect-container')[0].scrollHeight;
        var search = _el.siblings().find('.multiselect-search').val();
        _el.empty();


        list.orderBy(_columns.text).orderBy(_columns.selected, "desc");

        if (!isMultiple && allowEmpty) {
            _el.append(insertObject(emptyOptionTemplate, {}));
        }

        list.forEach(function (item) {
            var $option = insertObject(optionTemplate, item);

            if (item[_columns.selected])
                $option.attr("select", "select");

            _el.append($option);
        });

        if (isMultiple) {
            _el.multiselect('select', oldItems.map(function (item) {
                return item[_columns.value]
            }) || []);
        } else {
            if (oldItems.length) {
                _el.multiselect('select', oldItems[0][_columns.value]);
            } else {
                _el.multiselect('select', '');
            }
        }

        if (list.length || oldItems.length) _el.multiselect('rebuild');

        if (search && search.length > 0) {
            _el.siblings().find('.multiselect-search').val(search);
            _el.siblings().find('.multiselect-search').focus();
        } else {
            _el.siblings().find('.multiselect-container').scrollTop(scrollLength);
        }

        _el.siblings().find('.multiselect-container').off('scroll').on('scroll', function (e) {
            var elem = $(e.currentTarget);
            if (elem[0].scrollHeight - elem.scrollTop() <= elem.outerHeight() + 20) {
                refreshedTimeout.add(function () {
                    oldItems = [];
                    if (_el.val() && _el.val().length) {
                        if (isMultiple)
                            _el.val().forEach(function (selectedVal) {
                                oldItems.push(list.getByProp(_columns.value, selectedVal));
                            });
                        else oldItems.push(list.getByProp(_columns.value, _el.val()));
                    }
                    _pageable.next();
                }, 100, null, uniqueIdentifier);
            }
        });

        _el.siblings().find('.multiselect-search').off('keyup').on('keyup', function (e) {
            var filterValue = _el.siblings().find('.multiselect-search').val();
            refreshedTimeout.add(function () {
                oldItems = [];
                if (_el.val() && _el.val().length) {
                    if (isMultiple)
                        _el.val().forEach(function (selectedVal) {
                            oldItems.push(list.getByProp(_columns.value, selectedVal));
                        });
                    else oldItems.push(list.getByProp(_columns.value, _el.val()));
                }
                _pageable.setFilter(filterValue);
            }, 1000, null, uniqueIdentifier);
        });

        if (wasInit == false && init) {
            wasInit = true;
            init();
        }
    }

    Object.defineProperties(_el.data(), {
        clear: {
            get: function () {
                return function () {
                    _el.find('option:selected').each(function () {
                        $(this).prop('selected', false);
                    })
                    _el.multiselect('refresh');
                }
            }
        }
    })
}

jQuery.prototype.juMultiselect = function (obj) {
    var _el = $(this);
    var spinner = '<div class="sk-spinner sk-spinner-fading-circle">' +
        '<div class="sk-circle1 sk-circle"></div>' +
        '<div class="sk-circle2 sk-circle"></div>' +
        '<div class="sk-circle3 sk-circle"></div>' +
        '<div class="sk-circle4 sk-circle"></div>' +
        '<div class="sk-circle5 sk-circle"></div>' +
        '<div class="sk-circle6 sk-circle"></div>' +
        '<div class="sk-circle7 sk-circle"></div>' +
        '<div class="sk-circle8 sk-circle"></div>' +
        '<div class="sk-circle9 sk-circle"></div>' +
        '<div class="sk-circle10 sk-circle"></div>' +
        '<div class="sk-circle11 sk-circle"></div>' +
        '<div class="sk-circle12 sk-circle"></div>' +
        '</div>';

    obj.maxHeight = 200;
    obj.buttonWidth = '100%';
    obj.enableFiltering = obj.filtering || false;
    obj.enableCaseInsensitiveFiltering = obj.filtering || false;
    obj.filterPlaceholder = window.viewModel.multiselectOptions.filterPlaceholder;
    obj.nonSelectedText = "";
    obj.buttonText = function (options, select) {
        if (options.length === 0) {
            return obj.placeholder;
        } else if (options.length > obj.chosen) {
            return options.length + ' ' + window.viewModel.multiselectOptions.chosenPlaceholder;
        } else {
            var labels = [];
            options.each(function () {
                if ($(this).attr('label') !== undefined) {
                    labels.push($(this).attr('label'));
                } else {
                    labels.push($(this).html());
                }
            });
            return labels.join(', ') + '';
        }
    }
    _el.multiselect(obj);
    var dropdown = _el.parent().find('[data-toggle="dropdown"]');
    dropdown.dropdown();

    _el.closest('.bootstrap-multiselect-container').find('*').click(function (e) {
        e.stopPropagation();
    })
    _el.closest('.modal-dialog').bind('click_important', function (e) {
        _el.parent().find('.btn-group').removeClass('open');
    });

    if (obj.loading == true)
        _el.siblings('.btn-group').find('button').html(spinner);
}

jQuery.prototype.juStart = function (obj) {
    var count = obj.count;
    var element = $(this);

    var noneimage = "data:image/jpg;base64, /9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAZAAA/+4ADkFkb2JlAGTAAAAAAf/bAIQAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQICAgICAgICAgICAwMDAwMDAwMDAwEBAQEBAQECAQECAgIBAgIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMD/8AAEQgAEQARAwERAAIRAQMRAf/EAHQAAAIDAQAAAAAAAAAAAAAAAAIEAwcICgEBAQAAAAAAAAAAAAAAAAAAAAEQAAEEAgEABQ0AAAAAAAAAAAUCAwQGAQcAMWGRE3MS0kOzFFTEFRbWF5cIEQEAAQQBBQAAAAAAAAAAAAAAAfARITFRYXGxwdH/2gAMAwEAAhEDEQA/AOzy/f0YybC2asapFW6XbFyJlWTZJgCQAD1IiiTgeZITPqNYsqspX2FOusx0Q3crlIbS5jDalKwtbPNV8O+y+t9nEtUUVATZYS2Sa9U09yKv8GEybHIqiksZGRTzcWao/Gng3H8wEq9icQ/GZYc7xbq14SGgfyhWPcr3+q9ofZ/Api6Lrdf3mzKbBFyRI7rhySYC1UW+UKWg2iwQIFVnyYba2RkJwWLGkWFk5jkWOllbTL8jGERkYkddlrYjQ3zQmzbQ1PXTdWsdc+Xrt5N6o26ChlhdqHDhRGrWCJNFzClVtaQcSGTU0qLMlphyXW3XEsvpj5xRqTy8dXbnzeBD6Zzw4vrnuSPc+UgLvRE8X4Z/lU3wP//Z";

    var selectedimage = "data:image/jpg;base64, /9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAZAAA/+4ADkFkb2JlAGTAAAAAAf/bAIQAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQICAgICAgICAgICAwMDAwMDAwMDAwEBAQEBAQECAQECAgIBAgIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMD/8AAEQgAEQARAwERAAIRAQMRAf/EAIgAAAEFAAAAAAAAAAAAAAAAAAcBAwQJCgEAAgMBAAAAAAAAAAAAAAAAAwgEBgkHEAABBAIBAQMNAAAAAAAAAAADAgQFBgEHAAgSM3MRIaLSE1OzVLTWF5cJEQACAQMCBAAOAwAAAAAAAAACAwEEBQYSBwAREwghMVFxkcEiMnJTFNSVFrHC0v/aAAwDAQACEQMRAD8A11bq64aW3grnTNZJthr6gr2p5nH1ddVqOp8ml0iOmpF2izLiZrMtANVmK2AliXBXgxJJ2R5UrCO7zd4+EY/aLrjGHFcDzoRZTAZ0rKYKZ3OVsYc1MKZBpjWSx6MxLRCCiAmShp9t+23Jq+4W/IMq+iHFy01ErCoB51K4HWsB6EsVodOmDKXDMKkiiZLTEiHQfX1rbXlTj9b7de2Vu7gH5omsWVnEnsjR5VlqEuBYyKYwricxKQqTqj0pQyJ7Vs2CTtqIteMc92D7ysOsuGUmH7lnXLu1EUpRUAk6hbKWJiECZBqd1VxMp5QotQLAtRHJRxd91u2TKr/kDsqwYKMqOqCGOQTRQQ1PhFxBrgVdNunrcyYOkjMdMDA87Gfy1VPkNifprbf2Nx8f3yx/JvP4i6/ZcKT+k3X5tr/J2v7viqf+i+urRaN264i9La/k7BsG1VORd3IlcYnJh+ybSjGKrEjYnCcoio7Eels7Bh+6WDyiykZCZQISUZ596u3Fxy/c6x0O31mbV5lWW9p1RU4TMmsWAumY8vAoOnpcHWbIexAgRyKwEXT7Vcyt2P7f3aqze6KpsZo6xYU0PMeQHIMY8ED4WHJyay6S4nkXMxHURzIf6MdC3rX3WBAQvUHrR+wegrdllaS6mQIf13NriRx75pKwsuwM8r80+jooLtQ8JKZTcmUk7KCjHlNH7ZNnsgwbuGpLVunY3IeFHUtoicMGiapWhgOS5cnTuNa4YQxBnKz9vkLAEouW/W5lhyrZKquG2t2WxBVdOuqhc6H/AEzNYEtqjgXqA2SuJmRHWMaOcrI4nQJ5veemn1ea5aT8heiP88Zpc48seiOGA94TwBfEccEHvM8w+viQ33R+Kf4HhCd4jxzfRr4ZnjV5/UXAw8R/B/YeJvC8R+P/2Q==";

    element.hide();
    element.each(function (i, e) {
        var element = $(e);

        element.val(0);
        for (var i = 0; i < count; i++) {
            element.parent().append('<img class="center" role="ju-star-value" value="' + (i + 1) + '" src="' + noneimage + '">');
        }

        element.siblings('[role="ju-star-value"]').on('mouseover', function (e) {
            var target = $(e.target);
            target.attr('src', selectedimage);

            var max = Number(target.attr('value'));
            target.siblings('[role="ju-star-value"]').each(function (i, e) {
                var target = $(e);
                if (Number(target.attr('value')) > max)
                    return;

                target.attr('src', selectedimage);
            });
        });

        element.siblings('[role="ju-star-value"]').on('mouseleave', function (e) {
            var target = $(e.target);
            target.attr('src', target.data('selected') ? selectedimage : noneimage);

            var max = Number(target.attr('value'));
            target.siblings('[role="ju-star-value"]').each(function (i, e) {
                var target = $(e);
                if (Number(target.attr('value')) > max)
                    return;

                target.attr('src', target.data('selected') ? selectedimage : noneimage);
            });
        });

        element.siblings('[role="ju-star-value"]').on('click', function (e) {
            var target = $(e.target);
            var selected = target.data('selected') ? false : true;
            target.data('selected', selected);
            target.attr('src', selected ? selectedimage : noneimage);

            var max = Number(target.attr('value'));
            target.siblings('[role="ju-star-value"]').each(function (i, e) {
                var target = $(e);
                if (Number(target.attr('value')) > max)
                    return;

                target.data('selected', selected);
                target.attr('src', selected ? selectedimage : noneimage);
            });

            element.val(selected ? target.attr('value') : 0);
        });
    });

    return element;
}

//start: Date tipinde başlama tarihi ayarlanacağı zaman kullanılır
//end: Date tipinde bitiş tarihi ayarlanacağı zaman kullanılır.
//Div içerindeki input elementlerin role attribute'leri start ve end olmalıdır.
//  var startLimit = obj.startlimit; Haritaların başlangıç tarihi belirlenir
//  var endLimit = obj.endlimit; Haritaların bitiş tarihi belirlenir
jQuery.prototype.juDatePicker = function (obj) {
    obj = obj || {};
    var $this = $(this);
    var startElement = $this.find("input[role='start']");
    var endElement = $this.find("input[role='end']");
    var startLimit = obj.startlimit;
    var endLimit = obj.endlimit
    var startValue = obj.start;
    var endValue = obj.end;


    function setDatePicker(obj) {
        var _this = this;
        var _element = obj.element;
        var _date = obj.date || null;

        Object.defineProperties(_this, {
            value: {
                get: function () {
                    return _element.data("datepicker").getDate();
                },
                set: function (date) {
                    _element.datepicker("setDate", date);
                }
            },
            formatted: {
                get: function () {
                    return _element.val() ? _this.value.toISOString() : null;
                }
            },
        })

        Object.defineProperties(_element.data(), {
            value: {
                get: function () {
                    return function () {
                        return _this.value;
                    }
                }
            },
            formatted: {
                get: function () {
                    return function () {
                        return _this.formatted;
                    }
                }
            }
        });

        function initDate() {
            _element.datepicker({
                autoclose: true,
                language: window.viewModel.currentLanguage,
                clearBtn: true,
                todayBtn: 'linked',
                weekStart: 1,
                todayHighlight: 'TRUE',
                endDate: endLimit,
                startDate: startLimit
            });

            if (_date)
                _this.value = _date;

            /* _element.data("formatted", function () {
                 return _this.formatted;
             });*/
        }
        initDate();
    }

    if (startElement && startElement.length) {
        new setDatePicker({
            element: startElement,
            date: startValue
        })
    };
    if (endElement && endElement.length) {
        new setDatePicker({
            element: endElement,
            date: endValue
        })
    };

    if (startElement && startElement.length && endElement && endElement.length) {
        if (endValue) {
            startElement.datepicker('setEndDate', endValue)
        }

        if (startValue) {
            endElement.datepicker('setStartDate', startValue || null)
        }

        startElement.datepicker().on('changeDate', function (e) {
            if (e.date > endElement.datepicker("getDate"))
                startElement.val("").datepicker("update")
            else
                endElement.datepicker('setStartDate', e.date || null)
        });

        endElement.datepicker().on('changeDate', function (e) {
            if (e.date < startElement.datepicker("getDate"))
                endElement.val("").datepicker("update")
            else
                startElement.datepicker('setEndDate', e.date || null)
        });
    }

    Object.defineProperties($this.data(), {
        start: {
            get: function () {
                return startElement;
            }
        },
        end: {
            get: function () {
                return endElement;
            }
        },
        clear: {
            get: function () {
                return function () {
                    startElement.val("");
                    endElement.val("");
                }
            }
        }
    });
}

jQuery.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

jQuery.fn.deserializeObject = function (data) {
    if (data.length === undefined)
        data = Object.keys(data).map(function (key) {
            return {
                name: key,
                value: data[key]
            };
        });

    data.forEach(function (item) {
        var $el = $('[name="' + item.name + '"]'),
            type = $el.attr('type');

        switch (type) {
            case 'checkbox':
                if ($el.length > 1) {
                    var values = item.value.length > 0 ? item.value : [item.value];
                    $el.each(function (i, el) {
                        if (values.indexOf($(el).val()) > -1)
                            $(el).prop('checked', true);
                        else
                            $(el).prop('checked', false);
                    })
                } else {
                    $el.prop('checked', true);
                }
                break;
            case 'radio':
                $el.filter('[value="' + item.value + '"]').prop('checked', true);
                break;
            default:
                $el.val(item.value);
        }
    });
}

jQuery.fn.setAHrefPost = function () {
    var container = $(this);
    container.on('click', '[post-request]', function (e) {
        e.preventDefault();
        var aTag = $(e.target);

        if (aTag.is('a[href]') == false)
            aTag = aTag.closest("a[href]");

        var url = aTag.attr('href');

        var ajx = new juAjax({
            url: url,
            success: function (response) {
                if (response.Message)
                    toastr.success(response.Message);
            },
            error: function (response) {
                if (response.Message)
                    toastr.error(response.Message);
            }
        });
    });
}

var $jQueryAttr = jQuery.prototype.attr;
jQuery.fn.attr = function () {
    var str = $jQueryAttr.apply(this, arguments);
    return typeof (str) == 'string' && (new RegExp(/(<[\w]+)/).test(str) || str.indexOf("\"") > -1) ? str.htmlEncode() : str;
}

var refreshedTimeout = {
    list: [],
    add: function (fn, timeout, prs, id) {
        var _this = this;
        if (fn && typeof fn == 'function') {
            var model = {
                id: fn.prototype.constructor.name + id,
                name: fn.prototype.constructor.name,
                prs: prs,
                timer: null,
                timeout: timeout || 3000,
                fn: fn,
            };
            var oldmodel = this.list.filter(function (item) {
                return item.id == model.id;
            });
            if (oldmodel.length > 0) {
                _this.remove(oldmodel[0]);
                this.add(model.fn, model.timeout, model.prs, id);
            } else {
                model.timer = window.setTimeout(function () {
                    model.fn(model.prs);
                    _this.remove(model);
                }, model.timeout);
                this.list.push(model);
            }
        }
    },
    remove: function (model) {
        window.clearTimeout(model.timer);
        this.list = this.list.filter(function (item) {
            return item.id != model.id;
        });
    }
};

function JuAjaxForMap(obj) {
    if (obj && obj.map) {
        var bounding = obj.map.GetBounding();
        obj.data = obj.data || {};

        obj.data.bounds = {
            "firstLong": bounding[0][0],
            "firstLat": bounding[0][1],
            "secondLong": bounding[1][0],
            "secondLat": bounding[1][1]
        }

        new juAjax(obj);
    }

}

var juAjax = (function () {
    var _options = {
        _ajaxCount: 0,
        ajaxes: [],
        start: null,
        end: null
    };

    Object.defineProperties(_options, {
        ajaxCount: {
            get: function () {
                return _options._ajaxCount;
            },
            set: function (value) {
                if (_options._ajaxCount == 0 && value > 0 && _options.start)
                    _options.start();
                else if (_options._ajaxCount > 0 && value == 0 && _options.end)
                    _options.end();
                _options._ajaxCount = value;
            }
        }
    });

    function juAjax(obj) {
        var _ajx = this;
        this.defineId = obj.defineId;
        this.id = obj.id || ''.getGuid();
        this.operationTime = new Date();

        this.url = obj.crossDomain ? obj.url : ((window.viewModel && window.viewModel.origin) || '') + obj.url;
        this.type = obj.type || 'POST';
        this.contentType = obj.contentType || "application/json";
        this.dataType = obj.dataType || 'json';
        this.headers = obj.headers;
        this.success = obj.success;
        this.error = obj.error;
        this.xhr = obj.xhr;
        this.data = obj.data;
        this.always = obj.always;
        this.crossDomain = obj.crossDomain;

        this.define = obj.define || '';

        this.others = [];

        function callOthers(response) {
            response.id = _ajx.id;
            if (_ajx.success)
                _ajx.success(response, _ajx.operationTime, _ajx.define);

            _ajx.others.forEach(function (other) {
                if (other.success)
                    other.success(response, other.operationTime, other.define);
            });
        }

        function callOtherFails(response) {
            response.id = _ajx.id;
            if (_ajx.error)
                _ajx.error(response, _ajx.operationTime, _ajx.define);

            _ajx.others.forEach(function (other) {
                if (other.error)
                    other.error(response, other.operationTime, other.define);
            });
        }

        function callOtherAlways(response) {
            response.id = _ajx.id;
            if (_ajx.always)
                _ajx.always(response, _ajx.operationTime, _ajx.define);

            _ajx.others.forEach(function (other) {
                if (other.always)
                    other.always(response, other.operationTime, other.define);
            });
        }

        var runAjax = function _runAjaxFunction() {
            var oldAjax = null;
            // define Id'nin olduğu durumlarda eski işlemler artık callbackfunctionları çalıştırılmamalıdır.
            if (_ajx.defineId) {
                oldAjax = _options.ajaxes.filter(function (e) {
                    return e.defineId == _ajx.defineId;
                })[0];
                if (oldAjax)
                    oldAjax.success = oldAjax.error = oldAjax.always = null;
                oldAjax = null;
            } else {
                oldAjax = _options.ajaxes.filter(function (e) {
                    return e.id == _ajx.id || (e.url == _ajx.url && JSON.stringify(e.define) == JSON.stringify(_ajx.define));
                })[0];
            }

            if (oldAjax == null) {
                setTimeout(function () {
                    _options.ajaxCount = _options.ajaxCount + 1;
                }, 1000)

                this.ajax = $.ajax((function () {
                    var _opt = {
                        "url": _ajx.url,
                        "type": _ajx.type,
                    };

                    if (_ajx.data != null) {
                        _opt.data = JSON.stringify(_ajx.data);
                        _opt.contentType = _ajx.contentType;
                        _opt.dataType = _ajx.dataType
                    }

                    if (_ajx.xhr) {
                        _opt.xhr = _ajx.xhr;
                        _opt.processData = false;
                        _opt.contentType = false;
                    }

                    if (_ajx.headers)
                        _opt.headers = _ajx.headers;

                    return _opt;
                })());
                this.ajax.done(function (response) {
                    if (response.Status) {
                        callOthers(response);
                    } else {
                        callOtherFails(response);
                    }
                    _options.ajaxes = _options.ajaxes.removeByProp('id', _ajx.id);
                });
                this.ajax.fail(function (response, textStatus, err) {
                    try {
                        response.Message = window.page.messages.ajax_fail + ', ';
                    } catch (e) {
                        response.Message = '';
                    }
                    response.Message += err;
                    callOtherFails(response);
                    _options.ajaxes = _options.ajaxes.removeByProp('id', _ajx.id);
                });
                this.ajax.always(function (response, textStatus, err) {
                    callOtherAlways(response);
                    _options.ajaxCount = _options.ajaxCount - 1;
                    _options.ajaxes = _options.ajaxes.removeByProp('id', _ajx.id);

                    if (!response.Status && response.RedirectUrl)
                        setTimeout(function () {
                            window.location.href = response.RedirectUrl;
                        }, 1500);
                });
                _options.ajaxes.push(_ajx);
            } else {
                oldAjax.others.push(_ajx);
            }
        }

        if (_ajx.defineId) {
            refreshedTimeout.add(function () {
                runAjax();
            }, 1000, null, _ajx.defineId);
        } else {
            runAjax();
        }
    }


    juAjax.prototype.setOptions = function (obj) {
        obj = obj || {};
        _options.start = typeof (obj.start) == 'function' ? obj.start || null : null;
        _options.end = typeof (obj.end) == 'function' ? obj.end || null : null;
    }

    return juAjax;
})();

var juFormData = function (obj) {
    obj = obj || {};
    try {
        var _modal = obj.modal,
            form = obj.form;

        var _form = form || (_modal && _modal.find('form'));
        var _columns = obj.columns;
        var messages = obj.messages || {};
        var rules = obj.rules || {};
        var ignore = obj.ignore || ':hidden:not(".multiselect"):not("[type=\'file\']")';

        var _exist = (function () {
            var _exist = [];
            for (var p in _columns) {
                if (_form.find('.form-control[name="' + _columns[p] + '"]').length > 0) {
                    _exist.push(_columns[p]);
                }
            }
            return _exist;
        })();

        if (_exist.length == 0)
            return {};

        var _messages = {};
        var _rules = {};

        _exist.forEach(function (column) {
            if (messages[column])
                _messages[column] = messages[column];
            if (rules[column])
                _rules[column] = rules[column];
        });

        if ($.validator.methods["regex"] == null)
            $.validator.addMethod("regex", function (value, element, regexpr) {
                return regexpr.test(value);
            });

        if ($.validator.methods["filesize"] == null)
            $.validator.addMethod("filesize", function (value, element, size) {
                if ($(element).get(0).files[0] == null) return true;
                return size ? $(element).get(0).files[0].size < size : true;
            });

        if ($.validator.methods["filetype"] == null)
            $.validator.addMethod("filetype", function (value, element, filetype) {
                if ($(element).get(0).files[0] == null) return true;
                return filetype ? filetype.test($(element).get(0).files[0].name) : true;
            });

        if ($.validator.methods["maxfilepixel"] == null)
            $.validator.addMethod("maxfilepixel", function (value, element, options) {
                if ($(element).get(0).files[0] == null) return true;

                var result = false,
                    image, base64, height = options.height,
                    width = options.width;
                var imgOpt = $(element).data('image');
                if (imgOpt != null)
                    result = imgOpt.width <= width && imgOpt.height <= height;
                else
                    result = true;
                return result;
            });

        if ($.validator.methods["absolutefilepixel"] == null)
            $.validator.addMethod("absolutefilepixel", function (value, element, options) {
                if ($(element).get(0).files[0] == null) return true;

                var result = false,
                    image, base64, height = options.height,
                    width = options.width;
                var imgOpt = $(element).data('image');
                if (imgOpt != null)
                    result = imgOpt.width == width && imgOpt.height == height;
                else
                    result = true;
                return result;
            });


        _form.validate({
            rules: _rules,
            messages: _messages,
            ignore: ignore,
            highlight: function (element) {
                if ($(element).is('.multiselect'))
                    $(element).siblings('.btn-group').addClass('has-error');
                $(element).closest('.form-group').addClass('has-error');
            },
            unhighlight: function (element) {
                if ($(element).is('.multiselect'))
                    $(element).siblings('.btn-group').removeClass('has-error');
                $(element).closest('.form-group').removeClass('has-error');
            },
            errorPlacement: function (error, element) {
                if (element.is('.multiselect')) {
                    element.siblings('.btn-group').after(error);
                } else {
                    element.after(error);
                }
            },
        });

        if (_form.valid()) {
            var _data = {};
            _exist.forEach(function (column) {
                var value = undefined;
                var element = _form.find('.form-control[name="' + column + '"]');
                var type = element.attr('type');

                switch (type) {
                    case "checkbox":
                        value = element.is(":checked");
                        break;
                    case "text":
                        value = element.val().trim();
                        break;
                    case "email":
                        value = element.val().trim();
                        break;
                    case "password":
                        value = element.val();
                        break;
                    default:
                        value = element.val ? element.val() : undefined;
                        break;
                }

                if (value !== undefined) {
                    _data[column] = value;
                }
            });
            return _data;
        } else
            return null;
    } catch (e) {
        return null;
    }
}

var juInsertObject = function (reference, inject, func) {
    reference = reference || {};
    inject = inject || {};
    for (var key in inject) {
        if (func && typeof (func) == 'function') {
            var result = func(key, inject[key], reference);
            if (result === true || result === false) {
                if (result === true)
                    reference[key] = inject[key];
            } else if (result !== undefined)
                reference[key] = result;
        } else
            reference[key] = inject[key];
    }
    return reference;
}

var juCloneObject = function (obj) {
    return JSON.parse(JSON.stringify(obj));
}

var INARPost = function () {

    this.PostObject = function (URL, PostObject, SuccessCallBack) {
        jQuery.ajax({
            type: "POST",
            url: URL,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(PostObject),
            success: SuccessCallBack,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                toastr.warning("İşlem sırasında hata meydana geldi.");
            },
        });
    }

    if (this instanceof INARPost) {
        return this.INARPost;
    } else {
        return new INARPost();
    }
}

var INARConfirmation = function () {

    this.Verify = function (operationResult, successAction, errorAction) {
        if (operationResult.Status) {
            toastr.success(operationResult.Message);
            if (successAction != null)
                successAction();
        } else {
            toastr.error(operationResult.Message);
            if (errorAction != null)
                errorAction();
        }
    }

    this.VerifyNoMessage = function (operationResult, successAction, errorAction) {
        if (operationResult.Status) {
            if (successAction != null)
                successAction();
        } else {
            if (errorAction != null)
                errorAction();
        }
    }

    if (this instanceof INARConfirmation) {
        return this.INARConfirmation;
    } else {
        return new INARConfirmation();
    }
}
var setId = function (obj, cb) {
    obj = obj || {};
    var id = obj.id || null;
    var modal = obj.modal;
    var pageable = obj.pageable;

    var script = id ? $('script#' + id) : $(document.currentScript);
    var container = script.prev('#unknown');
    container.attr('ju-container', '');
    var id = ''.getGuid();

    var $modal;

    if (modal) {
        $('body').append('<div id="modal-' + id + '" class="modal inmodal" aria-hidden="true"><div class="modal-dialog"> <div class="modal-content animated fadeInDown"></div> </div></div>');

        $modal = $('#modal-' + id);

        var _htmlFn = $modal.html;
        $modal.html = function (html) {
            $modal.find('.modal-content').html(html);
            $modal.find('.ju-star').juStart({ count: 5 });
            var $dialog = $modal.find('.modal-dialog');
            if (html.data) {
                $dialog.data({
                    insertObject: html.data('insertObject')
                });
            }
        }

        $modal.clearClick = function () {
            $modal.unbind('click');
        }

        $modal.defineClick = function (cb) {
            $modal.clearClick();
            $modal.bind('click', function (e) {
                $modal.hide();
                if (cb)
                    cb();
            });
        }

        $modal.hide = function () {
            $modal.modal('hide');
            $('body').css('padding-right', '0px');
        }

        $modal.setPanel = function () {
            var initial = true;

            $modal.find('[data-toggle="collapse"]').click(function (e) {
                var colBtn = $(e.target);
                var panel = colBtn.closest('.panel');

                function process(expanded, panel) {
                    panel = panel || colBtn.closest('.panel');
                    expanded = expanded == null ? JSON.parse(panel.attr('expanded')) : expanded;

                    var cont = panel.find('.panel-body');
                    if (expanded)
                        cont.slideUp();
                    else
                        cont.slideDown();
                    panel.attr('expanded', JSON.stringify(!expanded));
                }

                if (initial) {
                    var expanded = JSON.parse(panel.attr('expanded'));

                    if (!expanded) {
                        process(!expanded);
                    }
                } else {
                    process();

                    var hasGroup = colBtn.closest('.panel-group').length > 0;

                    if (hasGroup) {
                        panel.siblings('[expanded="true"]').each(function (i, e) {
                            process(true, $(e));
                        });
                    }
                }
            });

            setTimeout(function () {
                $modal.find('[data-toggle="collapse"]').each(function (i, e) {
                    $(e).trigger("click");
                });
                initial = false;
            }, 100);
        }

        $modal.setFileButton = function () {
            $modal.find('.btn-file').unbind('click').bind('click', function (e) {
                $(e.target).closest('.btn-file').siblings('[type="file"]').trigger('click');
            });

            $modal.find('.btn-file').siblings('[type="file"]').hide();

            var $file = $modal.find('.btn-file').siblings('[type="file"]');
            if (!$file.data('setFileButtonChanged')) {
                $file.data({
                    setFileButtonChanged: true
                });
                $file.bind('change', function (e) {
                    setTimeout(function () {
                        if (!$(e.target).attr('name'))
                            $(e.target).attr('name', $(e.target).siblings('[type="hidden"]').attr('name'));
                        $(e.target).siblings('[type="hidden"]').attr('name', '');
                    }, 0);
                });
            }
        }

        $modal.getShowOptions = function () {
            return $modal.find('.modal-dialog').data('showOptions');
        }

        $modal.actions = function () {
            console.log("not loaded actions for a modal");
        }

        $modal.show = function (obj) {
            var $dialog = $modal.find('.modal-dialog'),
                insertedObj = $dialog.data('insertObject');
            obj = obj || {};
            var hasForm = obj.hasForm || false,
                columns = obj.columns,
                messages = obj.messages,
                rules = obj.rules,
                ignore = obj.ignore,
                switchFn = obj.switchFn,
                cancelFn = obj.cancelFn,
                onloadFn = obj.onloadFn,
                size = obj.size || '',
                externalform = obj.externalform || false,
                bodyClick = obj.bodyClick,
                focus = obj.focus,
                ignoreEnter = obj.ignoreEnter;

            var hasCloseBtn = $dialog.find("button.close-modal").length;
            closable = obj.closable == null ? !hasCloseBtn : obj.closable,


                $dialog.data('showOptions', obj);

            $dialog.classList = ["modal-dialog"];
            if (size)
                $dialog.addClass(size);
            else
                $dialog.removeClass("modal-lg").removeClass("modal-sm");

            if (onloadFn)
                $modal.off('shown.bs.modal').on('shown.bs.modal', function (e) {
                    onloadFn($modal);
                });
            $modal.modal('show');

            $modal.setPanel();

            $modal.setFileButton();

            function _defineClick() {
                if (closable)
                    $modal.defineClick(cancelFn);
            };

            if (closable)
                _defineClick();
            else {
                $modal.clearClick();
                //$modal.find('.close-modal').hide();
            }

            $dialog.unbind('click').bind('click', function (e) {
                $dialog.trigger("click_important");
                e.stopPropagation();
                if (bodyClick)
                    bodyClick(e);
            });

            $modal.find('.close-modal').bind('click', function (e) {
                $modal.hide();
                if (cancelFn)
                    cancelFn();
            });

            function done(e) {

                $modal.clearClick();

                var formData = {};
                if (hasForm) {
                    formData = juFormData({
                        modal: $modal,
                        columns: columns,
                        messages: messages,
                        rules: rules,
                        ignore: ignore
                    });
                }

                if (formData) {
                    var _btn;
                    if ($(e.target).hasClass("ladda-button"))
                        var _btn = $(e.target);
                    else _btn = $modal.find('button[role].ladda-button');

                    var _l_btn = _btn.ladda();
                    _l_btn.ladda('start');

                    function endCallback(status) {
                        _defineClick();
                        _l_btn.ladda('stop');
                        if (status)
                            $modal.hide();
                    }

                    var role = _btn.attr('role');
                    if (switchFn)
                        switchFn({
                            role: role,
                            formData: formData,
                            inserted: insertedObj,
                            columns: columns,
                            cb: endCallback
                        });

                } else {
                    _defineClick();
                }
            }

            $modal.find('.ladda-button').unbind('click').bind('click', done);

            $modal.actions = function () {
                $modal.find('.i-checks').iCheck({
                    checkboxClass: 'icheckbox_square-green',
                    radioClass: 'iradio_square-green',
                }).on('ifChanged', onChanged);

                $modal.find('[data-toggle="tab"]').off("click").on("click", function (e) {
                    $(e.target).tab('show');
                });

                $modal.find(".ys").juToggleGroup();
            }
            $modal.actions();

            function onChanged(e) {
                var name = $(e.target).attr('name');
                var onchanged = obj[name + '_changed'];
                if (onchanged)
                    onchanged.call(this, e, $modal);
            }

            $modal.find('input.form-control').change(onChanged);

            $modal.on("keypress", "form", function (e) {
                if (e.keyCode == 13 && !$modal.find(':focus').is("textarea")) {
                    e.preventDefault();
                }
                return e.keyCode != 13 || ignoreEnter;
            });

            $modal.off('keyup').on('keyup', function (e) {
                if (e.which == 13 && !ignoreEnter && !$modal.find(':focus').is("textarea")) {
                    e.preventDefault();

                    done(e);
                }
            });

            if (focus) {
                $modal.find("[name='" + focus + "']").focus();
            }
        }
    }

    _options = {
        order: null,
        url: null,
        success: null,
        error: null,
        skip: 0,
        take: 20,
        filterValue: null,
        page: true,
        search: true,
        data: [],
    }

    var _pageable = {
        setConfiguration: function (options) {
            var _pageable = new pageableModel(_options);
            if (options.url) {
                _pageable.options.url = options.url;
                _pageable.options.order = options.order;
                _pageable.options.error = options.error;
                _pageable.options.success = options.success;
                if (options.skip != null) _pageable.options.skip = options.skip;
                if (options.take != null) _pageable.options.take = options.take;
            }
            _pageable.getList({
                data: options.data
            });

            return _pageable;
        },
    };

    function pageableModel(obj) {
        this.options = Object.create(obj);
        this.dirty = true;
    }

    pageableModel.prototype.getList = function (obj) {
        obj = obj || {};
        var _this = this,
            data = obj.data;
        _this.options.url = obj.url || _this.options.url;

        if (_this.options.url && _this.dirty) {
            var jsonData = {
                take: _this.options.take,
                skip: _this.options.skip,
                order: _this.options.order,
                filter: _this.options.filterValue
            };
            if (data)
                Object.keys(data).forEach(function (key) {
                    if (jsonData[key] == null)
                        jsonData[key] = data[key];
                });

            this.ajx = new juAjax({
                define: String.prototype.getGuid(),
                data: jsonData,
                url: _this.options.url,
                success: function (response) {
                    _this.dirty = response.Data.data.length == _this.options.take;
                    if (response.Data.data.length >= 0) {
                        _this.options.data = _this.options.data.concat(response.Data.data);
                        if (_this.ajx.id == response.id)
                            _this.options.success(_this.options.data);
                    }
                },
                error: function (response) {
                    if (_this.ajx.id = response.id)
                        _this.options.error(response.Message);
                }
            });
        }
    }

    pageableModel.prototype.setFilter = function (obj) {
        var _this = this;
        _this.clear();

        if (typeof obj == "string") {
            _this.options.filterValue = obj;
            _this.getList(obj);
        } else if (obj.filter != null) {
            _this.options.filterValue = obj.filter;
            _this.getList(obj);
        } else if (obj.filterData) {
            _this.dirty = true;
            _this.getList({ data: obj.filterData });
        }


    }

    pageableModel.prototype.next = function (obj) {
        var _this = this;
        if (_this.options.data.length > 0 && _this.dirty) {
            _this.options.skip += _this.options.take;
            _this.getList(obj);
        }
    }

    pageableModel.prototype.clear = function () {
        var _this = this;
        _this.options.filterValue = null;
        _this.options.skip = 0;
        _this.options.data = [];

        _this.dirty = true;
    }

    container.attr('id', id);
    return {
        id: id,
        container: container,
        modal: $modal,
        pageable: _pageable,
        pagetype: window.pageType,
        searchbar: window.searchBar,
        orderbar: window.orderBar,
        paging: window.paging
    };
}