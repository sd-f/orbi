package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectTypeCategory;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeCategoryEntity;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = GameObjectTypeCategoryEntity.class)
@MappingConfig(GameObjectTypeCategoryMappper.class)
public abstract class GameObjectTypeCategoryRepository extends AbstractEntityRepository<GameObjectTypeCategory, Long> {

}
