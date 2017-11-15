const mongoose = require('mongoose');

let TaskSchema = mongoose.Schema({
    title: {type: 'string', required: 'true'},
    comments: {type: 'string', required: 'true'}
});

let Task = mongoose.model('Task', TaskSchema);

module.exports = Task;