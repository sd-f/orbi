package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObjectTypeCategory;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectTypeCategoryRepository;

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
        return repository.findAll();
    }

}
