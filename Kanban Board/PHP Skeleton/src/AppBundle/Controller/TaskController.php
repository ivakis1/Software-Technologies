<?php

namespace AppBundle\Controller;

use AppBundle\Entity\Task;
use AppBundle\Form\TaskType;
use Doctrine\ORM\Mapping as ORM;
use Sensio\Bundle\FrameworkExtraBundle\Configuration\Route;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;

/**
 * @ORM\Entity
 * @ORM\Table(name="task_controller")
 */
class TaskController extends Controller
{
    /**
     * @param Request $request
     * @Route("/", name="index")
     * @return \Symfony\Component\HttpFoundation\Response
     */
    public function index(Request $request)
    {
        $db = $this->getDoctrine()->getRepository(\AppBundle\Entity\Task::class);

        $openTasks = $db->findBy(['status' => 'Open']);
        $inProgressTasks = $db->findBy(['status' => 'In Progress']);
        $finishedTasks = $db->findBy(['status' => 'Finished']);

        return $this->render(':task:index.html.twig', [
            "openTasks" => $openTasks,
            "inProgressTasks" => $inProgressTasks,
            "finishedTasks" => $finishedTasks
        ]);

    }

    /**
     * @param Request $request
     * @Route("/create", name="create")
     * @return \Symfony\Component\HttpFoundation\Response
     */
    public function create(Request $request)
    {
        $task = new Task();
        $form = $this->createForm(TaskType::class, $task);
        $form->handleRequest($request);

        if ($form->isValid() && $form->isSubmitted()){

            $em = $this->getDoctrine()->getManager();

            $em->persist($task);
            $em->flush();

            return $this->redirect("/");
        }

        return $this->render(
            'task/create.html.twig',
            ['task' => $task, 'form' => $form->createView()]);
    }

    /**
     * @Route("/edit/{id}", name="edit")
     *
     * @param $id
     * @param Request $request
     * @return \Symfony\Component\HttpFoundation\Response
     */
    public function edit($id, Request $request)
    {
        $db = $this->getDoctrine()->getRepository(Task::class);

        $task = $db->find($id);

        if($task == null){
            return $this->redirect("/");
        }

        $form = $this->createForm(TaskType::class, $task);
        $form->handleRequest($request);

        if($form->isSubmitted() && $form->isValid()){

            $em = $this->getDoctrine()->getManager();

            $em->merge($task);
            $em->flush();

            return $this->redirect("/");
        }

        return $this->render(":task:edit.html.twig", ["task" => $task, 'form' => $form->createView()]);

    }
}
