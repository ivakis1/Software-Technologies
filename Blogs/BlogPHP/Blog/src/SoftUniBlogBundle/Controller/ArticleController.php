<?php

namespace SoftUniBlogBundle\Controller;

use Sensio\Bundle\FrameworkExtraBundle\Configuration\Route;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Security;
use SoftUniBlogBundle\Entity\Article;
use SoftUniBlogBundle\Form\ArticleType;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;

class ArticleController extends Controller
{
    /**
     * @Route("/articles/create", name="article_create")
     * @Security("is_granted('IS_AUTHENTICATED_FULLY')")
     * @return \Symfony\Component\HttpFoundation\Response
     */
    public function create(Request $request)
    {
        $article = new Article();
        $form = $this->createForm(ArticleType::class, $article);
        $form->handleRequest($request);

        $loggedUser = $this->getUser();

        if ($form->isValid()) {
            $article->setAuthor($loggedUser);

            $enitityManager = $this->getDoctrine()->getManager();
            $enitityManager->persist($article);
            $enitityManager->flush();

            return $this->redirectToRoute("article_list");
        }

        return $this->render('articles/create.html.twig',
            [
                'form' => $form->createView()
            ]
        );
    }

    /**
     * @Route("/articles/list", name="article_list")
     * @return \Symfony\Component\HttpFoundation\Response
     */
    public function listArticles()
    {
        $articleRepository = $this->getDoctrine()
            ->getRepository(Article::class);

        $allArticles = $articleRepository->findAll();

        return $this->render('articles/list.html.twig',
            [
                'articles' => $allArticles
            ]
        );
    }

    /**
     * @Route("/articles/{id}", name="article_show")
     * @param $id
     * @return \Symfony\Component\HttpFoundation\Response
     */
    public function show($id)
    {
        $articleRepository = $this
            ->getDoctrine()
            ->getRepository(Article::class);

        $article = $articleRepository->find($id);

        return $this->render('articles/show.html.twig',
            [
                'article' => $article
            ]
        );

    }
}
