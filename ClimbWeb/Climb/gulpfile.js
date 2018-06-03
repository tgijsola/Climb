"use strict";

var gulp = require("gulp"),
    fs = require("fs"),
    del = require("del"),
    ts = require("gulp-typescript"),
    less = require("gulp-less");

const cleanTask = "clean";
const lessTask = "less";
const typeScriptTask = "typescript";

var paths = {
    styles: "ClientApp/styles/**/*.less",
    scripts: "ClientApp/scripts/**/*.ts",
    output: "wwwroot/dist"
};

gulp.task(cleanTask, function () {
    return del("wwwroot/dist/**/*");
});

gulp.task(lessTask, function () {
    return gulp.src(paths.styles)
        .pipe(less())
        .pipe(gulp.dest(paths.output));
});

gulp.task(typeScriptTask,
    function() {
        return gulp.src(paths.scripts)
            .pipe(ts({
                noImplicitAny: true,
                outFile: "script.js"
            }))
            .pipe(gulp.dest(paths.output));
    });

gulp.watch(paths.styles, gulp.parallel(lessTask));
gulp.watch(paths.styles, gulp.parallel(typeScriptTask));

gulp.task("default", gulp.series(cleanTask, gulp.parallel(lessTask, typeScriptTask)));