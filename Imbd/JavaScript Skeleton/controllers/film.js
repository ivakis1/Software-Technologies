const Film = require('../models/Film');

module.exports = {
	index: (req, res) => {

		Film.find({}).then(films => {
			res.render('film', {films: films})
		});
	},
	createGet: (req, res) => {

		res.render('film/create');
	},
	createPost: (req, res) => {

		let film = req.body;

		Film.create(film).then(film => {
			res.redirect("/");
		})
	},
	editGet: (req, res) => {

        let id = req.params.id;

        Film.findById(id).then(film => {

        	if(film === undefined){
                res.redirect("/");
            }

            res.render("film/edit", film)
		});
	},
	editPost: (req, res) => {

        let id = req.params.id;
        let filmArguements = req.body;

        Film.findById(id).then(film => {

            if(film === undefined){
                res.redirect("/");
            }

			Film.findByIdAndUpdate(id, filmArguements).then(res.redirect("/"));

        });
	},
	deleteGet: (req, res) => {
        let id = req.params.id;

        Film.findById(id).then(film => {

            if(film === undefined){
                res.redirect("/");
            }

            Film.findById(id).then(res.render("film/delete", film));

        });

	},
	deletePost: (req, res) => {
        let id = req.params.id;

        Film.findByIdAndRemove(id).then(film => {
        	if(film === undefined){
                res.redirect("/");
            }

            res.redirect("/");
        });
    }
};