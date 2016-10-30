package foundation.softwaredesign.orbi.service.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectTypeCategory;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectTypeCategoryRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class GameObjectTypeCategoryService {

    @Inject
    GameObjectTypeCategoryRepository repository;

    public List<GameObjectTypeCategory> loadAll() {
        return repository.findAllOrderByOrdering();
    }

}
