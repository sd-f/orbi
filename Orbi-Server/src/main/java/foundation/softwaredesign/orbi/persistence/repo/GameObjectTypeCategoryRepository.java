package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.GameObjectTypeCategory;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeCategoryEntity;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.mapping.MappingConfig;

import javax.validation.constraints.NotNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository(forEntity = GameObjectTypeCategoryEntity.class)
@MappingConfig(GameObjectTypeCategoryMappper.class)
public abstract class GameObjectTypeCategoryRepository extends AbstractEntityRepository<GameObjectTypeCategory, Long> {

}
