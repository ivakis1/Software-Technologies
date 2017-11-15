const Product = require('../models/Product');

module.exports = {
	index: (req, res) => {

        Product.find().then(product => {
            res.render('product/index', {'entries': product});

        });

    	},
	createGet: (req, res) => {

		res.render("product/create");
	},
	createPost: (req, res) => {

		let product = req.body;

		Product.create(product).then(product => {
			res.redirect('/');
		})
	},
	editGet: (req, res) => {

		let id = req.params.id;


		Product.findById(id).then(product => {

            if(product === undefined){
                res.redirect("/");
            }

            res.render("product/edit", product)

		});
	},
	editPost: (req, res) => {

        let id = req.params.id;
        let productArguements = req.body;

        Product.findById(id).then(product => {

            if(product === undefined){

                res.redirect("/");

                return;
            }
            Product.findByIdAndUpdate(id, productArguements).then(res.redirect("/"));
        })
	}
};