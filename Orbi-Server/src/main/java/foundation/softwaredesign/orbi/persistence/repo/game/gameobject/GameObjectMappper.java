package foundation.softwaredesign.orbi.persistence.repo.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.gameobject.ai.AiProperties;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectEntity;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectTypeService;
import foundation.softwaredesign.orbi.service.game.server.DateConverter;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import javax.inject.Inject;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.bind.Unmarshaller;
import java.io.StringReader;
import java.io.StringWriter;
import java.util.Date;
import java.util.logging.Logger;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObjectMappper extends SimpleQueryInOutMapperBase<GameObjectEntity, GameObject> {

    @Inject
    UserService userService;
    @Inject
    GameObjectTypeService gameObjectType;

    @Inject
    DateConverter date;

    @Override
    protected Object getPrimaryKey(GameObject gameObject) {
        return gameObject.getId();
    }

    @Override
    protected GameObject toDto(GameObjectEntity objectEntity) {
        GameObject gameObject = new GameObject();
        gameObject.setId(objectEntity.getId());
        gameObject.getTransform().getGeoPosition().setLatitude(objectEntity.getLatitude());
        gameObject.getTransform().getGeoPosition().setLongitude(objectEntity.getLongitude());
        gameObject.getTransform().getGeoPosition().setAltitude(objectEntity.getAltitude());
        gameObject.getTransform().getRotation().setY(objectEntity.getRotationY());
        if (nonNull(objectEntity.getGameObjectType())) {
            gameObject.setType(new GameObjectTypeMappper().toDto(objectEntity.getGameObjectType()));
        }
        gameObject.setCreateDate(date.toString(objectEntity.getCreateDate()));
        gameObject.setIdentityId(objectEntity.getIdentity().getId());
        gameObject.setUserText(objectEntity.getUserText());
        gameObject.setName(objectEntity.getName());
        gameObject.setConstraints(objectEntity.getBodyConstraints());

        // TODO data structure
        AiProperties properties = getAiPropertiesDeserialized(objectEntity);
        if (nonNull(properties)) {
            gameObject.setAiProperties(properties);
        }


        return gameObject;
    }

    @Override
    protected GameObjectEntity toEntity(GameObjectEntity gameObjectEntity, GameObject gameObject) {
        GameObjectEntity newGameObjectEntity = gameObjectEntity;
        if (isNull(gameObjectEntity)) {
            newGameObjectEntity = new GameObjectEntity();
        }
        if (isNull(newGameObjectEntity.getId())) {
            newGameObjectEntity.setIdentity(userService.getIdentity());
            newGameObjectEntity.setCreateDate(new Date());
        }
        if (nonNull(gameObject.getTransform())) {
            if (nonNull(gameObject.getTransform().getGeoPosition())) {
                newGameObjectEntity.setLatitude(gameObject.getTransform().getGeoPosition().getLatitude());
                newGameObjectEntity.setLongitude(gameObject.getTransform().getGeoPosition().getLongitude());
                newGameObjectEntity.setAltitude(gameObject.getTransform().getGeoPosition().getAltitude());
            }
            if (nonNull(gameObject.getTransform().getRotation())) {
                newGameObjectEntity.setRotationY(gameObject.getTransform().getRotation().getY());
            }
        }
        newGameObjectEntity.setUserText(gameObject.getUserText());
        newGameObjectEntity.setGameObjectType(new GameObjectTypeMappper().toEntity(null, gameObjectType.loadByPrefab(gameObject.getType().getPrefab())));

        newGameObjectEntity.setName(gameObject.getName());
        newGameObjectEntity.setBodyConstraints(gameObject.getConstraints());

        if (gameObject.getType().getAi()) {
            newGameObjectEntity.setAiProperties(getAiPropertiesSerialized(gameObject));
        }

        return newGameObjectEntity;
    }

    private AiProperties getAiPropertiesDeserialized(GameObjectEntity objectEntity) {
        if (isNull(objectEntity.getAiProperties()) || (nonNull(objectEntity.getAiProperties()) && objectEntity.getAiProperties().isEmpty())) {
            return null;
        }
        AiProperties properties = null;
        StringReader sr = new StringReader(objectEntity.getAiProperties());
        try {
            JAXBContext context = JAXBContext.newInstance(AiProperties.class);
            Unmarshaller unmarshaller = context.createUnmarshaller();
            properties = (AiProperties) unmarshaller.unmarshal( sr );
        } catch (JAXBException e) {
            Logger.getLogger(GameObjectMappper.class.getSimpleName()).finest(e.getMessage());
        }
        return properties;
    }

    private String getAiPropertiesSerialized(GameObject gameObject) {
        if (isNull(gameObject.getAiProperties())) {
            return null;
        }
        StringWriter sw = new StringWriter();
        try {
            JAXBContext context = JAXBContext.newInstance(AiProperties.class);
            Marshaller marshaller = context.createMarshaller();
            marshaller.marshal(gameObject.getAiProperties(), sw );
        } catch (JAXBException e) {
            // slient
            Logger.getLogger(GameObjectMappper.class.getName()).finest(e.getMessage());
        }
        return sw.toString();
    }
}
