const Task = require('../models/Task');

module.exports = {

    index: (req, res) => {

        Task.find().then(task => {
            res.render('task/index', {'tasks': task});

        });
    },

    createGet: (req, res) => {

        res.render('task/create');
    },

    createPost: (req, res) => {

        let taskArgs = req.body;

        if (taskArgs.title === undefined || taskArgs.comments === undefined) {
            res.redirect('/');
            return;
        }

        Task.create(taskArgs).then(task => {
            res.redirect('/');
        });

    },

    deleteGet: (req, res) => {

        let id = req.params.id;

        if (!id) {
            res.redirect('/');
            return;
        }


        Task.findById(id).then(task => {

            if (!task) {
                res.redirect('/');
                return;
            }
            res.render('task/delete', task)
        })
    },

    deletePost: (req, res) => {

        let id = req.params.id;

        Task.findByIdAndRemove(id).then(task => {

            res.redirect('/')
        })
    }
};