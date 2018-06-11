"use strict";

const gulp = require("gulp"),
    fs = require("fs"),
    del = require("del"),
    ts = require("gulp-typescript"),
    less = require("gulp-less");

const prepareTask = "clean:prepare";
const postTask = "clean:post";
const lessTask = "less";
const cssConcatTask = "css:concat";
const typeScriptTask = "typescript";

const tsProject = ts.createProject('tsconfig.json');

const paths = {
    less: "ClientApp/styles/**/*.less",
    ts: "ClientApp/scripts/**/*.ts",
    tsx: "ClientApp/components/**/*.tsx",
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

//gulp.task(typeScriptTask + "react",
//    function() {
//        var tsResult = gulp.src(paths.tsx).pipe(tsProject());

//        return tsResult.js.pipe(gulp.dest(paths.output));
//        //return gulp.src(paths.tsx)
//        //    .pipe(ts({
//        //        noImplicitAny: true,
//        //        outFile: "react.js",
//        //        jsx: "react",
//        //        module: "esnext",
//        //        moduelResolution: "node",
//        //        target: "esnext",
//        //        sourceMap: true,
//        //        skipDefaultLibCheck: true,
//        //        strict: true
//        //    }))
//        //    .pipe(gulp.dest(paths.output));
//    });

gulp.task(typeScriptTask + "react",
    function() {
        return gulp.src(paths.tsx)
            .pipe(ts({
                noImplicitAny: true,
                module: 'commonjs',
                jsx: "react",
                outFile: 'output.js'
            }))
            .pipe(gulp.dest('built/local'));
    });

gulp.task("default",
    gulp.series(prepareTask,
        gulp.parallel(lessTask, typeScriptTask, typeScriptTask + "react"),
        postTask));

gulp.task("watch",
    function() {
        gulp.watch(paths.less, gulp.series(lessTask));
        gulp.watch(paths.ts, gulp.series(typeScriptTask));
    });