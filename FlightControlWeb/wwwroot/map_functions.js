"use strict";

/**
 * draws new airplane-icon on the map.
 * returns a refence to the new object
 * that can be passed to remove_from_map.
 * params format example:
 * { lat: 22.291, lng: 10.027 }
 */
function draw_airplane_icon_on_map(where, id, onclick) {

    const icon = {
        url: regular_plane_icon_url,
    };

    const marker = new google.maps.Marker({
        position: where,
        offset: '16',
        icon: icon,
        map: map,
    });

    google.maps.event.addDomListener(marker, 'click', onclick);

    return marker;
}

/**
 * draws new line on the map.
 * returns a refence to the new line
 * that can be passed to remove_from_map.
 * params format example:
 * { lat: 22.291, lng: 10.027 }, { lat: 78.291, lng: 153.027 }
 */
function draw_line_on_map(from, to) {

    const little_dash = { /* define a symbol using SVG path notation */
        path: 'M 0,-1 0,1',
        strokeOpacity: 1,
        scale: 4
    };

    return new google.maps.Polyline({
        path: [from, to],
        strokeOpacity: 0,
        icons: [{
            icon: little_dash,
            offset: '0',
            repeat: '20px'
        }],
        map: map
    });
}

function remove_from_map(thing) {
    thing.setMap(null);
}


function moveMarkerTo(marker, newPosition) {

    let options = {
        duration: 1000,
        easing: function (x, t, b, c, d) {
            /* jquery animation: swing (easeOutQuad) */
            return -c * (t /= d) * (t - 2) + b;
        }
    };

    window.requestAnimationFrame = window.requestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.msRequestAnimationFrame;

    window.cancelAnimationFrame = window.cancelAnimationFrame ||
        window.mozCancelAnimationFrame;

    /* save current position. prefixed to avoid name collisions.
     * separate for lat/lng to avoid calling lat()/lng() in every frame */
    marker.AT_startPosition_lat = marker.getPosition().lat();
    marker.AT_startPosition_lng = marker.getPosition().lng();
    var newPosition_lat = newPosition.lat();
    var newPosition_lng = newPosition.lng();

    // crossing the 180° meridian and going the long way around the earth?
    if (Math.abs(newPosition_lng - marker.AT_startPosition_lng) > 180) {
        if (newPosition_lng > marker.AT_startPosition_lng) {
            newPosition_lng -= 360;
        } else {
            newPosition_lng += 360;
        }
    }

    var animateStep = function (marker, startTime) {
        let ellapsedTime = (new Date()).getTime() - startTime;
        let durationRatio = ellapsedTime / options.duration; /* 0 - 1 */
        let easingDurationRatio = options.easing(
            durationRatio, ellapsedTime, 0, 1, options.duration);

        if (durationRatio < 1) {
            marker.setPosition({
                lat: (
                    marker.AT_startPosition_lat +
                    (newPosition_lat - marker.AT_startPosition_lat) *
                    easingDurationRatio
                ),
                lng: (
                    marker.AT_startPosition_lng +
                    (newPosition_lng - marker.AT_startPosition_lng) *
                    easingDurationRatio
                )
            });

            /* use requestAnimationFrame if it exists on this browser.
             * If not, use setTimeout with ~60 fps */

            if (window.requestAnimationFrame) {
                marker.AT_animationHandler =
                    window.requestAnimationFrame(function () {
                        animateStep(marker, startTime)
                    });
            }
            else {
                marker.AT_animationHandler =
                    setTimeout(function () {
                        animateStep(marker, startTime)
                    }, 17);
            }

        } else {
            marker.setPosition(newPosition);
        }
    }

    /* stop possibly running animation */
    if (window.cancelAnimationFrame) {
        window.cancelAnimationFrame(marker.AT_animationHandler);
    } else {
        clearTimeout(marker.AT_animationHandler);
    }

    animateStep(marker, (new Date()).getTime());
}