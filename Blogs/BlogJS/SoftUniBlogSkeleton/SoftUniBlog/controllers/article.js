const Article = require('mongoose').model('Article');
module.exports = {
    createGet: (req, res) => {
        res.render('article/create');
    },
    createPost: (req, res) => {
        let articleParts = req.body;

        let errorMessage = '';

        if (!req.isAuthenticated()) {
            errorMessage = "Sorry, you must be logged in!";
        } else if (!articleParts.title) {
            errorMessage = "Title is required!";
        } else if (!articleParts.content) {
            errorMessage = "Content is required!";
        }

        if (errorMessage) {
            res.render('article/create', {error: errorMessage});
            return;
        }

        let userId = req.user.id;

        articleParts.author = userId;

        Article.create(articleParts).then(article => {
            req.user.articles.push(article.id);
            req.user.save(err => {
                if (err) {
                    res.render('article/create',
                        {
                            error: err.message
                        });
                } else {
                    res.redirect('/');
                }
            });
        });
    },
    detailsGet: (req, res) => {
        let id = req.params.id;

        Article.findById(id).populate('author').then(article => {
            res.render('article/details', article)
        });
    }
};