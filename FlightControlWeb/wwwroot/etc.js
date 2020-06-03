"use strict";




function now_in_nice_format() {
  const date = new Date();
  const hours = date.getHours();
  let minutes = date.getMinutes();
  minutes = minutes < 10 ? '0' + minutes : minutes;
  const strTime = hours + ':' + minutes;
  return date.getDate() + "/" + (date.getMonth() + 1) + "/"
    + date.getFullYear() + ", " + strTime;
}