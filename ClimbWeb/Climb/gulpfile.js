"use strict";

const gulp = require("gulp"),
    fs = require("fs"),
    del = require("del"),
    ts = require("gulp-typescript"),
    less = require("gulp-less"),
    concatCss = require("gulp-concat-css");

const prepareTask = "clean:prepare";
const postTask = "clean:post";
const lessTask = "less";
const cssConcatTask = "css:concat";
const typeScriptTask = "typescript";

const paths = {
    less: "ClientApp/styles/**/*.less",
    ts: "ClientApp/scripts/**/*.ts",
    output: "wwwroot/dist",
    css: "wwwroot/dist/temp/*.css"
};

gulp.task(prepareTask,
    function() {
        return del("wwwroot/dist/**/*");
    });

gulp.task(postTask,
    function() {
        return del("wwwroot/dist/temp/**/*");
    });

gulp.task(lessTask,
    function() {
        return gulp.src(paths.less)
            .pipe(less())
            .pipe(gulp.dest(paths.output + "/temp"));
    });

gulp.task(cssConcatTask,
    function() {
        return gulp.src(paths.css)
            .pipe(concatCss("bundle.css"))
            .pipe(gulp.dest(paths.output));
    });

gulp.task(typeScriptTask,
    function() {
        return gulp.src(paths.ts)
            .pipe(ts({
                noImplicitAny: true,
                outFile: "script.js"
            }))
            .pipe(gulp.dest(paths.output));
    });

//gulp.watch(paths.styles, gulp.parallel(lessTask));
//gulp.watch(paths.styles, gulp.parallel(typeScriptTask));

gulp.task("default",
    gulp.series(prepareTask,
        gulp.parallel(gulp.series(lessTask, cssConcatTask),
            typeScriptTask),
        postTask));