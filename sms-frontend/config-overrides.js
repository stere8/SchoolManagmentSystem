const webpack = require('webpack');

module.exports = function override(config) {
  config.resolve.fallback = {
    ...config.resolve.fallback,
    https: require.resolve('https-browserify'),
    http: require.resolve('stream-http'),
    url: require.resolve('url/'),
    buffer: require.resolve('buffer/')
  };
  return config;
};
