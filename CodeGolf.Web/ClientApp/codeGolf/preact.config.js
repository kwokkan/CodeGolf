import { resolve } from "path";
import envVars from "preact-cli-plugin-env-vars";
import CopyWebpackPlugin from "copy-webpack-plugin";
import LodashModuleReplacementPlugin from "lodash-webpack-plugin";

export default function(config, env, helpers) {
    // Switch css-loader for typings-for-css-modules-loader, which is a wrapper
    // that automatically generates .d.ts files for loaded CSS
    helpers.getLoadersByName(config, "css-loader").forEach(({ loader }) => {
        loader.loader = "typings-for-css-modules-loader";
        loader.options = Object.assign(loader.options, {
            camelCase: true,
            banner:
                "// This file is automatically generated from your CSS. Any edits will be overwritten.",
            namedExport: true,
            silent: true
        });
    });

    config.plugins.push(
        new CopyWebpackPlugin([
            { context: `${__dirname}/src/assets`, from: `*.*` }
        ])
    );

    // helpers.getLoadersByName(config, "babel-loader").forEach(({ loader }) => {
    //     loader.options.plugins.push(babelPluginLodash)
    // });
    config.plugins.push(new LodashModuleReplacementPlugin({}));

    // Use any `index` file, not just index.js
    config.resolve.alias["preact-cli-entrypoint"] = resolve(
        process.cwd(),
        "src",
        "index"
    );
    config.resolve.alias["create-react-class"] =
        "preact-compat/lib/create-react-class";

    envVars(config, env, helpers);
    return config;
}
