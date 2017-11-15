const Task = require('../models/Task');

module.exports = {
    index: (req, res) => {

        let taskPromises = [
            Task.find({status: 'Open'}),
            Task.find({status: 'In Progress'}),
            Task.find({status: 'Finished'})];

        Promise.all(taskPromises).then(tasksResult => {
            res.render(
                'task/index',
                {
                    'openTasks': tasksResult[0],
                    'inProgressTasks': tasksResult[1],
                    'finishedTasks': tasksResult[2]
                });
        });
    },

    createGet: (req, res) => {
        res.render('task/create');
    },
    createPost: (req, res) => {

        let task = req.body;

       Task.create(task).then(task => {
           res.redirect('/');
       }).catch( err => {
           task.error = 'Cannot create task.';
           res.render('task/create', task)
           });
    },
    editGet: (req, res) => {
        let id = req.params.id;

        Task.findById(id).then(task => {
            if(task) {
                res.render('task/edit', task)
            }else {
                res.redirect('/');
            }
        }).catch(err => res.redirect('/'));
    },

    editPost: (req, res) => {
        let task = req.body;
        let taskId = req.params.id;

        Task.findByIdAndUpdate(taskId, task, {runValidators: true})
            .then(tasks => {res.redirect('/')})
            .catch(err => {
                task.id = taskId;
                task.error = "Cannot edit task."
                res.render('task/edit', task );
            });


    }
};