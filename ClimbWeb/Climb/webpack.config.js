const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CheckerPlugin = require("awesome-typescript-loader").CheckerPlugin;
const bundleOutputDir = "./wwwroot/dist";
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

var glob_entries = require('webpack-glob-entries');

module.exports = (env) => {
    console.log(env);
    const isDevBuild = !(env && env.prod);
    return [
        {
            stats: { modules: false },
            entry: glob_entries("./ClientApp/scripts/**/*.ts"),
            resolve: { extensions: [".ts", ".tsx"] },
            output: {
                path: path.join(__dirname, bundleOutputDir),
                filename: "[name].js",
                publicPath: "dist/"
            },
            module: {
                rules: [
                    { test: /\.tsx?$/, include: /ClientApp/, use: "awesome-typescript-loader" },
                    { test: /\.less?$/, include: /ClientApp/, use: ["style-loader", "css-loader", "less-loader"] }
                ]
            },
            plugins: [
                new CheckerPlugin()
            ].concat(isDevBuild
                ? [
                    // Plugins that apply in development builds only
                    new webpack.SourceMapDevToolPlugin({
                        filename: "[file].map", // Remove this line if you prefer inline source maps
                        moduleFilenameTemplate:
                            path.relative(bundleOutputDir,
                                "[resourcePath]") // Point sourcemap entries to the original file locations on disk
                    })
                ]
                : [
                    // Plugins that apply in production builds only
                    new UglifyJsPlugin(),
                    new ExtractTextPlugin("site.css")
                ])
        }
    ];
};