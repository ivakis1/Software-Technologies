package softuniBlog.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import softuniBlog.entity.Article;

/**
 * Created by User on 3.8.2017 г..
 */
public interface ArticleRepository extends JpaRepository<Article, Integer> {
}
