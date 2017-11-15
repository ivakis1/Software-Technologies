package softuniBlog.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import softuniBlog.bindingModel.ArticleBingingModel;
import softuniBlog.entity.Article;
import softuniBlog.entity.User;
import softuniBlog.repository.ArticleRepository;
import softuniBlog.repository.UserRepository;

@Controller
public class ArticleController {

    @Autowired
    private ArticleRepository articleRepository;

    @Autowired
    private UserRepository userRepository;

    @GetMapping("/article/create")
    @PreAuthorize("isAuthenticated()")
    public String create(Model model){

        model.addAttribute("view", "article/create");

        return "base-layout";
    }

    @PostMapping("/article/create")
    @PreAuthorize("isAuthenticated()")
    public String createProcess(ArticleBingingModel model){

        UserDetails userDetails = (UserDetails) SecurityContextHolder
                .getContext()
                .getAuthentication()
                .getPrincipal();

        User user = this.userRepository.findByEmail(userDetails.getUsername());

        Article article = new Article(
                model.getTitle(),
                model.getContent(),
                user
        );

        this.articleRepository.saveAndFlush(article);

        return "redirect:/";
    }

    @GetMapping("/article/{id}")
    public String details(Model model, @PathVariable Integer id){

        if(!this.articleRepository.exists(id)) return "redirect:/";

        Article article = this.articleRepository.findOne(id);

        model.addAttribute("view", "/article/details");
        model.addAttribute("article", article);

        return "base-layout";
    }

    @GetMapping("/article/edit/{id}")
    @PreAuthorize("isAuthenticated()")
    public String edit(@PathVariable Integer id, Model model ){

        Article article = this.articleRepository.findOne(id);

        if (article == null) return "redirect:/";

        model.addAttribute("article", article);
        model.addAttribute("view", "article/edit");

        return "base-layout";
    }

    @PostMapping("/article/edit/{id}")
    @PreAuthorize("isAuthenticated()")
    public String editProcess(@PathVariable Integer id, ArticleBingingModel articleBingingModel){

        if (!this.articleRepository.exists(id)){
            return "redirect:/";
        }

        Article article = this.articleRepository.findOne(id);

        article.setContent(articleBingingModel.getContent());
        article.setTitle(articleBingingModel.getTitle());

        this.articleRepository.saveAndFlush(article);

        return "redirect:/article/" + article.getId();
    }

    @GetMapping("/article/delete/{id}")
    @PreAuthorize("isAuthenticated()")
    public String delete(@PathVariable Integer id, Model model){

        Article article = this.articleRepository.findOne(id);

        if (article == null) return "redirect:/";

        model.addAttribute("article", article);
        model.addAttribute("view", "/article/delete");

        return "base-layout";
    }

    @PostMapping("/article/delete/{id}")
    @PreAuthorize("isAuthenticated()")
    public String deleteProcess(@PathVariable Integer id){

        Article article = this.articleRepository.findOne(id);

        if (article == null) return "redirect:/";

        this.articleRepository.delete(article);

        return "redirect:/";
    }

}
