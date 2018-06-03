"use strict";

var gulp = require("gulp"),
    fs = require("fs"),
    del = require("del"),
    ts = require("gulp-typescript"),
    less = require("gulp-less");

var paths = {
    styles: "ClientApp/styles/**/*.less",
    scripts: "ClientApp/scripts/**/*.ts",
    output: "wwwroot/dist"
};

gulp.task("clean", function () {
    return del("wwwroot/dist/**/*");
});

gulp.task("less", function () {
    return gulp.src(paths.styles)
        .pipe(less())
        .pipe(gulp.dest(paths.output));
});

gulp.task("typescript",
    function() {
        return gulp.src(paths.scripts)
            .pipe(ts({
                noImplicitAny: true,
                outFile: "script.js"
            }))
            .pipe(gulp.dest(paths.output));
    });

//gulp.watch(paths.styles, gulp.parallel("less"));

gulp.task("default", gulp.series("clean", gulp.parallel("less", "typescript")));