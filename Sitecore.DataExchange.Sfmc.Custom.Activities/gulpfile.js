let gulp = require("gulp") // or import * as gulp from 'gulp'

let del = require('del')
let ts = require('gulp-typescript')
var tsProject = ts.createProject("tsconfig.json");

async function postmongerjs() {
  return gulp.src('./node_modules/postmonger/postmonger.js')
    .pipe(gulp.dest('./Scripts/postmonger'))
}

async function salesforceDesignSystem() {
  return gulp.src('./node_modules/@salesforce-ux/design-system/assets/**/*')
    .pipe(gulp.dest('./Content/design-system'))
}

async function cleanScripts() {
  return await del('Scripts/**/*', { force: true })
}

async function cleanContent() {
  return await del('Content/**/*', { force: true });
}

async function tsc(){
  return tsProject.src().pipe(tsProject()).js.pipe(gulp.dest('./Scripts'));
}

exports.postmongerjs = postmongerjs;
exports.salesforceDesignSystem = salesforceDesignSystem;
exports.cleanScripts = cleanScripts;
exports.cleanContent = cleanContent;
exports.default = gulp.series(cleanContent, cleanScripts, salesforceDesignSystem, postmongerjs, tsc);